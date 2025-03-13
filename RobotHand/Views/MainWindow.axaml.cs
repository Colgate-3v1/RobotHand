using Avalonia.Controls;
using RobotHand.Services;

namespace RobotHand.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, System.EventArgs e)
        {
            RobotService.Instance.Disconnect();
        }
    }
}