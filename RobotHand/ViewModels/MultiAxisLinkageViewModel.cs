using fairino;
using ReactiveUI;
using RobotHand.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand.ViewModels
{
    public class MultiAxisLinkageViewModel : ViewModelBase
    {
        private RobotService _robotService;

        public MultiAxisLinkageViewModel(RobotService robotService)
        {
            _robotService = robotService;
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

        public void Apply()
        {
            var j1 = Convert.ToDouble(J1);
            var j2 = Convert.ToDouble(J2);
            var j3 = Convert.ToDouble(J3);
            var j4 = Convert.ToDouble(J4);
            var j5 = Convert.ToDouble(J5);
            var j6 = Convert.ToDouble(J6);
            ExaxisPos ePos = new ExaxisPos(0, 0, 0, 0);
            JointPos joint = new JointPos(j1, j2, j3, j4, j5, j6);
            _robotService.MoveJoint(joint, new DescPose(),
                0, 0, 100f, 100f, 100f, ePos, 0f, 0, new DescPose());
        }
        #endregion
    }
}
