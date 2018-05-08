using MvvmCross.Platform.IoC;
using Autofac.Extras.MvvmCross;
using Autofac;
using System.Collections.Generic;
using System;

namespace easyfft.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        IContainer _container;
        public App() : base()
        {
        }
        public void ConfigureContainer(Dictionary<Type, Type> mappedTypes)
        {
            var cb = new Autofac.ContainerBuilder();

            #region Registering Modules

            cb.RegisterModule<Modules.MainModule>();

            #endregion
            #region Adding Mapped Types to Container

            foreach (var mappedType in mappedTypes)
            {
                cb.RegisterType(mappedType.Value).As(mappedType.Key).SingleInstance();
            }

            #endregion
            _container = cb.Build();
        }
        public IContainer Container
        {
            get => _container;
        }
        public override void Initialize()
        {
            RegisterNavigationServiceAppStart<ViewModels.MainViewModel>();
        }
    }
}
