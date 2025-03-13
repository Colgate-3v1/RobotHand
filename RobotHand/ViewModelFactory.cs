using DryIoc;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand
{
    public class ViewModelFactory
    {
        private readonly IContainer _container;

        public ViewModelFactory(IContainer container)
        {
            _container = container;
        }

        public T Create<T>() where T : ReactiveObject
        {
            return _container.Resolve<T>();
        }
    }
}
