using DynamicData;
using NuclearMusicPlayer__AvaloniaExtension.Services;
using NuclearMusicPlayer__AvaloniaExtension.ViewModels.ListItems;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class RestoreBackupViewModel : ViewModelBase
    {
        public ObservableCollection<BackupToRestoreViewModel> Backups { get; set; } = new ObservableCollection<BackupToRestoreViewModel>();

        public BackupToRestoreViewModel SelectedBackup { get; set; }

        public RestoreBackupViewModel()
        {
            var backups = NuclearMusicPlayerService.Inst.GetBackups();
            foreach (var backup in backups)
            {
                Backups.Add(new BackupToRestoreViewModel() { Name = backup });
            }
        }

        public void GoBack()
        {
            NavigatorService.Pop();
        }

        public async Task RestoreBackup()
        {
            try
            {
                await NuclearMusicPlayerService.Inst.RestoreBackup(SelectedBackup.Name);
            }
            catch (Exception ex)
            {
                LogsService.WriteLine(ex.ToString());
            }
        }
    }
}
