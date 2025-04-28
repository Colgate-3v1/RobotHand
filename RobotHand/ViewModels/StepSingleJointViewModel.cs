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
    public class StepSingleJointViewModel : ViewModelBase
    {
        private readonly RobotService _robotService;

        private Timer timer;
        public StepSingleJointViewModel(RobotService robotService)
        {
            _robotService = robotService;
            timer = new Timer(GetCurrent, null, 0, 1000);
        }

        #region Свойства

        private string _j1;
        public string J1
        {
            get => _j1;
            set => this.RaiseAndSetIfChanged(ref _j1, value);
        }

        private string _j2;
        public string J2
        {
            get => _j2;
            set => this.RaiseAndSetIfChanged(ref _j2, value);
        }

        private string _j3;
        public string J3
        {
            get => _j3;
            set => this.RaiseAndSetIfChanged(ref _j3, value);
        }

        private string _j4;
        public string J4
        {
            get => _j4;
            set => this.RaiseAndSetIfChanged(ref _j4, value);
        }

        private string _j5;
        public string J5
        {
            get => _j5;
            set => this.RaiseAndSetIfChanged(ref _j5, value);
        }

        private string _j6;
        public string J6
        {
            get => _j6;
            set => this.RaiseAndSetIfChanged(ref _j6, value);
        }

        #endregion

        public async void PositiveStepCurrentJoint(string jointName)
        {
            var num = Convert.ToByte(jointName);
            _robotService.StartSteps(0, num, 1, 50, 100, 30);
        }
        
        public void NegativeStepCurrentJoint(string jointName)
        {
            var num = Convert.ToByte(jointName);
            _robotService.StartSteps(0, num, 0, 50, 100, 30);
        }

        public void StopMove()
        {
            _robotService.StopSteps();
        }

        public void GetCurrent(object o)
        {
            var j = _robotService.GetJoint();

            J1 = j.jPos[0].ToString();
            J2 = j.jPos[1].ToString();
            J3 = j.jPos[2].ToString();
            J4 = j.jPos[3].ToString();
            J5 = j.jPos[4].ToString();
            J6 = j.jPos[5].ToString();

        }
    }
}
