using NuclearMusicPlayer__AvaloniaExtension.Services;
using ReactiveUI;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            NavigatorService.Init(this);
            NavigatorService.Push(new HomeViewModel());
            NavigatorService.Push(new MergePlaylistsViewModel());
            NavigatorService.Push(new RestoreBackupViewModel());
        }


        private ViewModelBase _contentViewModel;
        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

    }

}