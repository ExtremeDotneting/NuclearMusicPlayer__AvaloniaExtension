using NuclearMusicPlayer__AvaloniaExtension.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearMusicPlayer__AvaloniaExtension.Services
{
    public static class NavigatorService
    {
        private static MainWindowViewModel _mainWindowViewModel;
        private static readonly Stack<ViewModelBase> _viewModelStack = new Stack<ViewModelBase>();

        public static void Init(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public static void Push(ViewModelBase viewModel)
        {
            if(_mainWindowViewModel.ContentViewModel!=null)
            {
                _viewModelStack.Push(_mainWindowViewModel.ContentViewModel);
            }
            _mainWindowViewModel.ContentViewModel = viewModel;
        }

        public static void Pop()
        {
            if(_viewModelStack.Count<=0)
            {
                throw new Exception("Navigation stack error.");
            }
            var viewModel = _viewModelStack.Pop();
            _mainWindowViewModel.ContentViewModel = viewModel;
        }
    }
}
