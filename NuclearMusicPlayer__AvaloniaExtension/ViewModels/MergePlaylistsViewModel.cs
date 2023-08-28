using NuclearMusicPlayer__AvaloniaExtension.Services;
using NuclearMusicPlayer__AvaloniaExtension.ViewModels.ListItems;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class MergePlaylistsViewModel : ViewModelBase
    {
        public ObservableCollection<PlaylistItemViewModel> Playlists { get; set; } = new ObservableCollection<PlaylistItemViewModel>();

        public MergePlaylistsViewModel()
        {
            Playlists.Add(new PlaylistItemViewModel { Name = "AAA", IsChecked = true });
            Playlists.Add(new PlaylistItemViewModel { Name = "bbb" });
        }

        public void GoBack()
        {
            NavigatorService.Pop();
        }
    }
}
