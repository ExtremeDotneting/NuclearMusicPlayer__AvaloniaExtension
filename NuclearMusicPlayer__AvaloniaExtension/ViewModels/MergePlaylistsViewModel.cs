using NuclearMusicPlayer__AvaloniaExtension.Services;
using NuclearMusicPlayer__AvaloniaExtension.ViewModels.ListItems;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class MergePlaylistsViewModel : ViewModelBase
    {
        public ObservableCollection<PlaylistItemViewModel> Playlists { get; set; } = new ObservableCollection<PlaylistItemViewModel>();

        private string _newPlaylistName;
        public string NewPlaylistName
        {
            get => _newPlaylistName;
            set => this.RaiseAndSetIfChanged(ref _newPlaylistName, value);
        }

        public MergePlaylistsViewModel()
        {
            var playlists = NuclearMusicPlayerService.Inst.GetPlaylists();
            foreach (var plName in playlists)
            {
                Playlists.Add(new PlaylistItemViewModel() { Name = plName });
            }
        }

        public void GoBack()
        {
            NavigatorService.Pop();
        }

        public void MergePlaylists()
        {
            try
            {
                var list = Playlists
                    .Where(r => r.IsChecked)
                    .Select(r => r.Name);
                NuclearMusicPlayerService.Inst.MetgePlaylists(list, NewPlaylistName);
            }
            catch (Exception ex)
            {
                LogsService.WriteLine(ex.ToString());
            }
        }
    }
}
