using Avalonia.Controls;
using ReactiveUI;
using RobotHand.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand.ViewModels
{
    /// <summary>
    /// ViewModel для работы вдоль одной оси или одного сустава по шагам
    /// </summary>
    public class StepViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly RobotService _robotService;

        public StepViewModel(NavigationService navigationService,
                             RobotService robotService)
        {
            _navigationService = navigationService;
            _robotService = robotService;
        }

        #region Свойства

        private string _coordinateSystem;
        public string CoordinateSystem
        {
            get => _coordinateSystem;
            set => this.RaiseAndSetIfChanged(ref _coordinateSystem, value);
        }

        private string _axisOrJoint;
        public string AxisOrJoint
        {
            get => _axisOrJoint;
            set => this.RaiseAndSetIfChanged(ref _axisOrJoint, value);
        }

        private string _directionOfMovement;
        public string DirectionOfMovement
        {
            get => _directionOfMovement;
            set => this.RaiseAndSetIfChanged(ref _directionOfMovement, value);
        }

        private string _percentSpeed;
        public string PercentSpeed
        {
            get => _percentSpeed;
            set => this.RaiseAndSetIfChanged(ref _percentSpeed, value);
        }

        private string _percentAcceleration;
        public string PercentAcceleration
        {
            get => _percentAcceleration;
            set => this.RaiseAndSetIfChanged(ref _percentAcceleration, value);
        }

        private string _maxDis;
        public string MaxDis
        {
            get => _maxDis;
            set => this.RaiseAndSetIfChanged(ref _maxDis, value);
        }

        public string InfoCoordinateSystem { get;} =
             "0 - указание по сустава\n" +
            "2 - указание в базовой системе координат\n" +
            "4 - указание в системе координат инструмента\n" +
            "8 - указание в системе координат заготовки";

        public string InfoAxisOrJoint { get; } =
            "1 - сустав 1 (или ось X)\n" +
            "2 - сустав 2 (или ось Y)\n" +
            "3 - сустав 3 (или ось Z)\n" +
            "4 - сустав 4 (или вращение вокруг оси X)\n" +
            "5 - сустав 5 (или вращение вокруг оси Y)\n" +
            "6 - сустав 6 (или вращение вокруг оси Z).";

        public string InfoDirectionOfMovement { get; } =
            "0 - отрицательное направление.\n" +
            "1 - положительное направление.";

        public string InfoPercentSpeed { get; } =
            "Скорость движения в процентах от максимальной скорости, значение от 0 до 100";

        public string InfoPercentAcceleration { get; } =
            "Ускорение движения в процентах от максимального ускорения, значение от 0 до 100";

        public string InfoMaxDis { get; } =
            "Максимальный угол или расстояние для одного шага движения. " +
            "Для суставов это значение в градусах, для координатных систем — в миллиметрах.";
        #endregion

        public void Start()
        {
            var type = Convert.ToByte(CoordinateSystem);
            var axis = Convert.ToByte(AxisOrJoint);
            var direction = Convert.ToByte(DirectionOfMovement);
            var speed = Convert.ToSingle(PercentSpeed);
            var acceleration = Convert.ToSingle(PercentAcceleration);
            var maxDis = Convert.ToSingle(MaxDis);
            _robotService.StartSteps(type, axis, direction, speed, acceleration, maxDis);
        }
    }
}
