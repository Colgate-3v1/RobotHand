using Avalonia.Controls;
using Avalonia.Input;
using RobotHand.Models;
using RobotHand.Services;
using System;

namespace RobotHand.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;

            KeyDown += KeyboardEventService.Instance.OnKeyDown;
            KeyUp += KeyboardEventService.Instance.OnKeyUp;
        }

        private void MainWindow_Closed(object? sender, System.EventArgs e)
        {
            CaptureStream.Instance.Stop();
            RobotService.Instance.Disconnect();
        }
    }
}