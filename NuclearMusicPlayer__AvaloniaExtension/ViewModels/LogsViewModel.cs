using ReactiveUI;

namespace NuclearMusicPlayer__AvaloniaExtension.ViewModels
{
    public class LogsViewModel : ViewModelBase
    {
        public static LogsViewModel Inst { get; } = new LogsViewModel();

        private string _logsText = "Logs...";
        public string LogsText
        {
            get => _logsText;
            set => this.RaiseAndSetIfChanged(ref _logsText,value);
        }

        LogsViewModel()
        {
        }

        public static void AddString(string str)
        {
            if (Inst == null)
                return;
            Inst.LogsText += "\n" + str;
        }

        public static void Clear()
        {
            if (Inst == null)
                return;
            Inst.LogsText ="";
        }
    }
}
