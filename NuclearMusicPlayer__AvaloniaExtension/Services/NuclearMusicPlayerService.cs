using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NuclearMusicPlayer__AvaloniaExtension.Services
{
    public class NuclearMusicPlayerService
    {
        public static NuclearMusicPlayerService Inst { get; } = new NuclearMusicPlayerService();

        public string BackupDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "playlistBackupsDir");
        public string ApiRootUrl { get; set; } = "http://localhost:3100";

        NuclearMusicPlayerService() { }

        public async Task MakeBacup()
        {
            var jsonStr = await SendHttpRequest(HttpMethod.Get, "nuclear/playlist");
            var dt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'_'mm'_'ss");
            var backupName = $"playlistBackup___{dt}";
            if (!Directory.Exists(BackupDirectory))
            {
                Directory.CreateDirectory(BackupDirectory);
            }
            var backupPath = Path.Combine(BackupDirectory, backupName) + ".json";
            File.WriteAllText(backupPath, jsonStr);
            LogsService.WriteLine($"Backup '{backupName}' saved to directory\n'{BackupDirectory}'.");
        }

        public IEnumerable<string> GetBackups()
        {
            var list = new List<string>();
            if (!Directory.Exists(BackupDirectory))
            {
                return list;
            }

            var files = Directory.GetFiles(BackupDirectory);
            foreach (var fullFileName in files)
            {
                var fileName = Path.GetFileName(fullFileName);
                list.Add(fileName);
            }
            return list;
        }

        public async Task RestoreBackup(string backupName)
        {
            var filePath = Path.Combine(BackupDirectory, backupName);
            var jsonStr = File.ReadAllText(filePath);
            var playlistsArray = JToken.Parse(jsonStr);
            var count = playlistsArray.Count();
            for (var i = 0; i < count; i++)
            {
                var playlist = playlistsArray[i];
                var name = playlist["name"]?.ToString();
                if (name != null)
                    await DeletePlaylist(name);
                await AddPlaylist(playlist);
            }
        }

        async Task AddPlaylist(JToken playlist)
        {
            LogsService.WriteLine($"Will add playlist.");
            var playlistJsonStr = playlist.ToString();
            await SendHttpRequest(HttpMethod.Post, $"nuclear/playlist", content: playlistJsonStr);
            LogsService.WriteLine($"Playlist added.");
        }

        async Task DeletePlaylist(string name)
        {
            LogsService.WriteLine($"Will delete playlist '{name}'.");
            name = UrlEncoder.Default.Encode(name);
            await SendHttpRequest(HttpMethod.Delete, $"nuclear/playlist/{name}");
            LogsService.WriteLine($"Playlist '{name}' is deleted.");
        }

        async Task<string> SendHttpRequest(
            HttpMethod httpMethod,
            string httpMethodPath,
            string content = null,
            string contentType = "application/json"
            )
        {
            if (!ApiRootUrl.EndsWith("/"))
            {
                ApiRootUrl += "/";
            }
            var url = ApiRootUrl + httpMethodPath;
            LogsService.WriteLine($"Will send http request to '{url}'.");
            LogsService.WriteLine($"Content: {content} .");
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(httpMethod, url);
            if (!string.IsNullOrWhiteSpace(content))
                request.Content = new StringContent(content, Encoding.UTF8, contentType);

            var response = await httpClient.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                LogsService.WriteLine($"Http request error: {response.StatusCode} .");
                throw new HttpRequestException("Response code is not 200.");
            }
            var strResponse = await response.Content.ReadAsStringAsync();
            var strRespForLogs = strResponse.Length > 100 
                ? strResponse.Remove(100) + "..." 
                : strResponse;
            LogsService.WriteLine($"Http response content:\n{strRespForLogs}");
            return strResponse;
        }
    }
}
