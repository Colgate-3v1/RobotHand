using RobotHand.Interfaces;
using RobotHand.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand.Services
{
    public class NavigationService
    {
        public event EventHandler<ViewModelBase>? PageChanged;

        private readonly ViewModelFactory _viewModelFactory;

        public NavigationService(ViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        private ViewModelBase? _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                if (value != null)
                    PageChanged?.Invoke(this, value);
            }
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            if (CurrentViewModel is INavigationPage fromPage)
            {
                fromPage.OnNavigatedFrom();
            }

            var vm = _viewModelFactory.Create<T>();

            if (vm == null)
                return;

            if (vm is INavigationPage toPage)
            {
                toPage.OnNavigatedTo();
            }

            CurrentViewModel = vm;
        }

        public void NavigateTo(ViewModelBase page)
        {
            if (CurrentViewModel is INavigationPage fromPage)
            {
                fromPage.OnNavigatedFrom();
            }

            if (page is INavigationPage toPage)
            {
                toPage.OnNavigatedTo();

                CurrentViewModel = page;
            }
        }
    }
}
