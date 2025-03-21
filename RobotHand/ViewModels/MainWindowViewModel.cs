using ReactiveUI;
using RobotHand.Services;
using System.Threading.Tasks;

namespace RobotHand.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly RobotService _robotService;

        public MainWindowViewModel(NavigationService navigationService, RobotService robotService)
        {
            _navigationService = navigationService;
            _robotService = robotService;

            //_robotService.Init();

            _navigationService.PageChanged += OnPageChanged;
            _navigationService.NavigateTo<StepSingleJointViewModel>();
        }

        #region Свойства
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }
        #endregion

        public void NavigateStep()
        {
            _navigationService.NavigateTo<StepViewModel>();
        }

        public void NavigateMultiAxis()
        {
            _navigationService.NavigateTo<MultiAxisLinkageViewModel>();
        }

        public void NavigateStepSingle()
        {
            _navigationService.NavigateTo<StepSingleJointViewModel>();
        }

        private void OnPageChanged(object? sender, ViewModelBase e)
        {
            CurrentPage = e;
        }
    }
}
