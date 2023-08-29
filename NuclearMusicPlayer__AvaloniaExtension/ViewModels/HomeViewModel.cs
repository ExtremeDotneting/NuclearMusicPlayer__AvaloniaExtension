using NuclearMusicPlayer__AvaloniaExtension.Services;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public string ApiUrl
        {
            get => NuclearMusicPlayerService.Inst.ApiRootUrl;
            set
            {
                NuclearMusicPlayerService.Inst.ApiRootUrl = value;
                this.RaisePropertyChanging();
            }
        }

        public async Task MakeBackup()
        {
            try
            {
                await NuclearMusicPlayerService.Inst.MakeBacup();
            }
            catch (Exception ex)
            {
                LogsService.WriteLine(ex.ToString());
            }
        }

        public async Task GoToLoadBackup()
        {
            NavigatorService.Push(new RestoreBackupViewModel());
        }
    }
}
