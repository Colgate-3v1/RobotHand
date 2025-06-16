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

        public byte Direction { get; set; } = 0;

        public bool Mode { get; set; } = false;


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
                    break;
                case Key.D:
                    _robotService.StopSteps();
                    break;
                case Key.LeftCtrl:
                    break;
                case Key.Space:
                    break;
            }
        }

        public void OnKeyDown(object? sender, KeyEventArgs e)
        {
            var key = e.Key;

            switch (key)
            {
                case Key.W:
                    _robotService.StartSteps(2, (byte)(Mode ? 5 : 2), Direction, 50, 100, 1000);
                    break;
                case Key.A:
                    _robotService.StartSteps(2, (byte)(Mode ? 4 : 1), Direction, 50, 100, 1000);
                    break;
                case Key.S:
                    Direction = (byte)(Direction == 0 ? 1 : 0);
                    break;
                case Key.D:
                    _robotService.StartSteps(2, (byte)(Mode ? 6 : 3), Direction, 50, 100, 1000);
                    break;
                case Key.LeftCtrl:
                    Mode = !Mode;
                    break;
                case Key.Space:
                    break;
            }
        }

    }
}
