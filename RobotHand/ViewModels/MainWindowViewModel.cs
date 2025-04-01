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

        private string _direction = "положительное";
        public string Direction
        {
            get => _direction;
            set => this.RaiseAndSetIfChanged(ref _direction, value);
        }

        private string _mode = "движение по осям";
        public string Mode
        {
            get => _mode;
            set => this.RaiseAndSetIfChanged(ref _mode, value);
        }

        #endregion

        public void NavigateStep()
        {
            _navigationService.NavigateTo<StepViewModel>();
        }

        public void NavigateMultiAxis()
        {
            _navigationService.NavigateTo<StepSingleAxisViewModel>();
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
