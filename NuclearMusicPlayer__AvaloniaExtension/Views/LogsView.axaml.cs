using Avalonia.Controls;
using NuclearMusicPlayer__AvaloniaExtension.ViewModels;

namespace NuclearMusicPlayer__AvaloniaExtension.Views
{
    public partial class LogsView : UserControl
    {
        public LogsView()
        {
            InitializeComponent();
            DataContext = LogsViewModel.Inst;
        }
    }
}
