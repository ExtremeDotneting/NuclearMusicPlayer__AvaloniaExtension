using NuclearMusicPlayer__AvaloniaExtension.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearMusicPlayer__AvaloniaExtension.Services
{
    public static class LogsService
    {
        public static void AddString(string str)
        {
            LogsViewModel.AddString(str);
        }

        public static void Clear()
        {
            LogsViewModel.Clear();
        }
    }
}
