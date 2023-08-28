using DynamicData;
using NuclearMusicPlayer__AvaloniaExtension.Services;
using NuclearMusicPlayer__AvaloniaExtension.ViewModels.ListItems;
using System.Collections.ObjectModel;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class RestoreBackupViewModel : ViewModelBase
    {
        public ObservableCollection<BackupToRestoreViewModel> Backups { get; set; } = new ObservableCollection<BackupToRestoreViewModel>();

        public RestoreBackupViewModel()
        {
            Backups.Add(new BackupToRestoreViewModel { Name = "backup__2022" });
            Backups.Add(new BackupToRestoreViewModel { Name = "backup__2023" });
            Backups.Add(new BackupToRestoreViewModel { Name = "backup__2024" });
        }

        public void GoBack()
        {
            NavigatorService.Pop();
        }
    }
}
