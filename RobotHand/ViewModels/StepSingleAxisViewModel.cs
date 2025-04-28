using ReactiveUI;
using RobotHand.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotHand.ViewModels
{
    public class StepSingleAxisViewModel : ViewModelBase
    {
        private readonly RobotService _robotService;

        private Timer timer;
        public StepSingleAxisViewModel(RobotService robotService)
        {
            _robotService = robotService;
            timer = new Timer(GetCurrent, null, 0, 1000);
        }

        #region Свойства

        private string _x;
        public string X
        {
            get => _x;
            set => this.RaiseAndSetIfChanged(ref _x, value);
        }

        private string _y;
        public string Y
        {
            get => _y;
            set => this.RaiseAndSetIfChanged(ref _y, value);
        }

        private string _z;
        public string Z
        {
            get => _z;
            set => this.RaiseAndSetIfChanged(ref _z, value);
        }

        private string _rx;
        public string RX
        {
            get => _rx;
            set => this.RaiseAndSetIfChanged(ref _rx, value);
        }

        private string _ry;
        public string RY
        {
            get => _ry;
            set => this.RaiseAndSetIfChanged(ref _ry, value);
        }

        private string _rz;
        public string RZ
        {
            get => _rz;
            set => this.RaiseAndSetIfChanged(ref _rz, value);
        }

        #endregion

        public async void PositiveStepCurrentAxis(string axisName)
        {
            var num = Convert.ToByte(axisName);
            _robotService.StartSteps(2, num, 1, 50, 100, 1000);
        }
        
        public void NegativeStepCurrentAxis(string axisName)
        {
            var num = Convert.ToByte(axisName);
            _robotService.StartSteps(2, num, 0, 50, 100, 1000);
        }

        public void StopMove()
        {
            _robotService.StopSteps();
        }

        public void GetCurrent(object o)
        {
            var pos = _robotService.GetPosition();

            X = pos.tran.x.ToString();
            Y = pos.tran.y.ToString();
            Z = pos.tran.z.ToString();
            RX = pos.rpy.rx.ToString();
            RY = pos.rpy.ry.ToString();
            RZ = pos.rpy.rz.ToString();
        }
    }
}
