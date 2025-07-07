using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand.Services
{
    public class KeyboardEventService
    {
        private readonly RobotService _robotService;

        public static KeyboardEventService Instance { get; private set; }

        public KeyboardEventService(RobotService robotService)
        {
            _robotService = robotService;
            Instance = this;
        }

        public void OnKeyUp(object? sender, KeyEventArgs e)
        {
            var key = e.Key;

            switch (key)
            {
                case Key.W:
                    _robotService.StopSteps();
                    break;
                case Key.A:
                    _robotService.StopSteps();
                    break;
                case Key.S:
                    _robotService.StopSteps();
                    break;
                case Key.D:
                    _robotService.StopSteps();
                    break;
                case Key.LeftCtrl:
                    _robotService.StopSteps();
                    break;
                case Key.LeftShift:
                    _robotService.StopSteps();
                    break;
                case Key.Right:
                    _robotService.StopSteps();
                    break;
                case Key.Left:
                    _robotService.StopSteps();
                    break;
                case Key.Up:
                    _robotService.StopSteps();
                    break;
                case Key.Down:
                    _robotService.StopSteps();
                    break;
                case Key.RightShift:
                    _robotService.StopSteps();
                    break;
                case Key.RightCtrl:
                    _robotService.StopSteps();
                    break;
            }
        }

        public void OnKeyDown(object? sender, KeyEventArgs e)
        {
            var key = e.Key;

            switch (key)
            {
                case Key.W:
                    _robotService.StartSteps(2, 1, 0, 25, 100, 1000);
                    break;
                case Key.A:
                    _robotService.StartSteps(2, 2, 0, 25, 100, 1000);
                    break;
                case Key.S:
                    _robotService.StartSteps(2, 1, 1, 25, 100, 1000);
                    break;
                case Key.D:
                    _robotService.StartSteps(2, 2, 1, 25, 100, 1000);
                    break;
                case Key.LeftCtrl:
                    _robotService.StartSteps(2, 3, 0, 25, 100, 1000);
                    break;
                case Key.LeftShift:
                    _robotService.StartSteps(2, 3, 1, 25, 100, 1000);
                    break;
                case Key.Right:
                    _robotService.StartSteps(2, 4, 0, 25, 100, 1000);
                    break;
                case Key.Left:
                    _robotService.StartSteps(2, 4, 1, 25, 100, 1000);
                    break;
                case Key.Up:
                    _robotService.StartSteps(2, 5, 0, 25, 100, 1000);
                    break;
                case Key.Down:
                    _robotService.StartSteps(2, 5, 1, 25, 100, 1000);
                    break;
                case Key.RightShift:
                    _robotService.StartSteps(2, 6, 0, 25, 100, 1000);
                    break;
                case Key.RightCtrl:
                    _robotService.StartSteps(2, 6, 1, 25, 100, 1000);
                    break;
            }
        }

    }
}
