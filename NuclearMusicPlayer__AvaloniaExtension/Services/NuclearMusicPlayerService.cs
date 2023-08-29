using DynamicData.Kernel;
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
        public const string DefaultThumbnail =
            "https://raw.githubusercontent.com/ExtremeDotneting/NuclearMusicPlayer__AvaloniaExtension/master/Docs/music.png";
        public string ConfigJsonPath { get; set; }

        NuclearMusicPlayerService()
        {
            ConfigJsonPath = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\nuclear\\config.json";
        }

        public void MakeBackup()
        {
            if (!File.Exists(ConfigJsonPath))
            {
                throw new FileNotFoundException(ConfigJsonPath);
            }

            var dt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'_'mm'_'ss");
            var backupName = $"backup___{dt}";
            if (!Directory.Exists(BackupDirectory))
            {
                Directory.CreateDirectory(BackupDirectory);
            }
            var backupPath = Path.Combine(BackupDirectory, backupName) + ".json";
            File.Copy(ConfigJsonPath, backupPath);
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

        public IEnumerable<string> GetPlaylists()
        {
            var config = GetCurrentConfigJToken();
            var playlists = config["playlists"];
            var resultList = playlists.Select(r => r["name"].ToString());
            return resultList;
        }

        public void MetgePlaylists(IEnumerable<string> sourcePlaylistsNames, string newPlaylistName)
        {
            if (string.IsNullOrWhiteSpace(newPlaylistName))
            {
                throw new ArgumentException(nameof(newPlaylistName));
            }

            var hashSet = new HashSet<string>();
            foreach(var plName in sourcePlaylistsNames)
            {
                hashSet.Add(plName);
            }

            var newPlaylist = JToken.FromObject(new object());
            newPlaylist["name"] = newPlaylistName;
            newPlaylist["id"] = Guid.NewGuid().ToString();
            newPlaylist["lastModified"] = 1690000000000;
            newPlaylist["tracks"] = new JArray();
            

            var config = GetCurrentConfigJToken();
            var playlists = config["playlists"];
            var playlistsCount = playlists.Count();
            for (var i = 0; i < playlistsCount; i++)
            {
                var pl = playlists[i];
                var name = pl["name"].ToString();
                if (!hashSet.Contains(name))
                {
                    continue;
                }

                var tracks = pl["tracks"];
                var tracksCount = tracks.Count();
                for (var j = 0; j < tracksCount; j++)
                {
                    var tr = tracks[j];
                    AddToJTokenArray(newPlaylist["tracks"], tr);
                }
            }

            //var newPlJson = newPlaylist.ToString();
            AddToJTokenArray(playlists, newPlaylist);
            var newJsonStr = config.ToString();
            File.WriteAllText(ConfigJsonPath, newJsonStr);
            LogsService.WriteLine($"New playlist was successfully created.");
        }

        public void NormalizePlaylists()
        {
            var config = GetCurrentConfigJToken();            
            var playlists=config["playlists"];
            var playlistsCount=playlists.Count();
            for (var i = 0; i < playlistsCount; i++)
            {
                var pl = playlists[i];
                var tracks = pl["tracks"];
                var tracksCount=tracks.Count();
                for(var j = 0; j < tracksCount; j++)
                {
                    var tr= tracks[j];
                    var thumbnail=tr["thumbnail"]?.ToString();
                    if (string.IsNullOrWhiteSpace(thumbnail))
                    {
                        tr["thumbnail"] = DefaultThumbnail;
                        var name = tr["name"].ToString();
                        LogsService.WriteLine($"For track '{name}' thumbnail was replaced.");
                    }
                }
            }
            var newJsonStr = config.ToString();
            File.WriteAllText(ConfigJsonPath, newJsonStr);
            LogsService.WriteLine($"All tracks was normalized.");
        }        

        public void RestoreBackup(string backupName)
        {
            var backupPath = Path.Combine(BackupDirectory, backupName);
            if (!File.Exists(backupPath))
            {
                throw new FileNotFoundException(ConfigJsonPath);
            }
            var text = File.ReadAllText(backupPath);
            File.WriteAllText(ConfigJsonPath, text);
            LogsService.WriteLine($"Backup '{backupName}' was restored.");
        }

        void AddToJTokenArray(JToken jToken, object item)
        {
            JArray jArray = (JArray)jToken;
            jArray.Add(item); 
        }

        JToken GetCurrentConfigJToken()
        {
            if (!File.Exists(ConfigJsonPath))
            {
                throw new FileNotFoundException(ConfigJsonPath);
            }
            var jsonStr= File.ReadAllText(ConfigJsonPath);
            var config = JToken.Parse(jsonStr);
            return config;
        }
    }
}
