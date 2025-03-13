using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand.Interfaces
{
    public interface INavigationPage
    {
        void OnNavigatedFrom();

        void OnNavigatedTo();
    }
}
