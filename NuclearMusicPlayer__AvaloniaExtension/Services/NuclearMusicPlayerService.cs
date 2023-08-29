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
        public string ConfigJsonPath { get; set; }

        NuclearMusicPlayerService()
        {
            ConfigJsonPath = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\nuclear\\config.json";
        }

        public async Task MakeBackup()
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

        public async Task RestoreBackup(string backupName)
        {
            var backupPath = Path.Combine(BackupDirectory, backupName);
            if (!File.Exists(backupPath))
            {
                throw new FileNotFoundException(ConfigJsonPath);
            }
            var text=File.ReadAllText(backupPath);
            File.WriteAllText(ConfigJsonPath, text);
            LogsService.WriteLine($"Backup '{backupName}' was restored.");
        }
    }
}
