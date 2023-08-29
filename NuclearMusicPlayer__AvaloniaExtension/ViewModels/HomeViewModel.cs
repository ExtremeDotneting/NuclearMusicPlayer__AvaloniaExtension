using NuclearMusicPlayer__AvaloniaExtension.Services;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public string ConfigJsonPath
        {
            get => NuclearMusicPlayerService.Inst.ConfigJsonPath;
            set
            {
                NuclearMusicPlayerService.Inst.ConfigJsonPath = value;
                this.RaisePropertyChanging();
            }
        }

        public void MakeBackup()
        {
            try
            {
                NuclearMusicPlayerService.Inst.MakeBackup();
            }
            catch (Exception ex)
            {
                LogsService.WriteLine(ex.ToString());
            }
        }

        public void GoToLoadBackup()
        {
            NavigatorService.Push(new RestoreBackupViewModel());
        }

        public void GoToMergePlaylist()
        {
            NavigatorService.Push(new MergePlaylistsViewModel());
        }        

        public void NormalizePlaylists()
        {
            try
            {
                NuclearMusicPlayerService.Inst.NormalizePlaylists();
            }
            catch (Exception ex)
            {
                LogsService.WriteLine(ex.ToString());
            }
            
        }
    }
}
