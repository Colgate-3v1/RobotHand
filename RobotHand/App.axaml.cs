using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DryIoc;
using ReactiveUI;
using RobotHand.Interfaces;
using RobotHand.Services;
using RobotHand.ViewModels;
using RobotHand.Views;
using Splat;
using Splat.DryIoc;
using System;

namespace RobotHand
{
    public partial class App : Application
    {
        protected IContainer? Container;

        public override void Initialize()
        {
            InitializeDI();
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeDI()
        {
            var container = new Container();


            container.Register<MainWindow>(Reuse.Singleton);
            container.Register<ViewModelFactory>(Reuse.Singleton);

            //Регистрация сервисов
            container.Register<NavigationService>(Reuse.Singleton);
            container.Register<RobotService>(Reuse.Singleton);


            //Регистрация ViewModels
            container.Register<MainWindowViewModel>(Reuse.Singleton);
            container.Register<StepViewModel>(Reuse.Singleton);
            container.Register<MultiAxisLinkageViewModel>(Reuse.Singleton);
            container.Register<StepSingleAxisViewModel>(Reuse.Singleton);
            container.Register<StepSingleJointViewModel>(Reuse.Singleton);


            var resolver = new DryIocDependencyResolver(container);

            Locator.SetLocator(resolver);
            container.RegisterInstance(resolver);

            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();
            //resolver.InitializeAvalonia();

            Container = container;
            //ViewModelLocator.Container = Container;
            var desktopServices = Container.ResolveMany<IDesktopAppService>();

            foreach (var service in desktopServices)
            {
                try
                {
                    service.StartAsync();
                }
                catch (Exception exc)
                {
                }
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var vm = Container?.Resolve<MainWindowViewModel>();
                desktop.MainWindow = Container?.Resolve<MainWindow>();
                desktop.MainWindow.DataContext = vm;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}