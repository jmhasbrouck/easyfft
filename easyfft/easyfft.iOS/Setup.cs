using MvvmCross.Core.ViewModels;
using System;
using easyfft.iOS.Services;
using easyfft.Core.Services;
using System.Collections.Generic;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using UIKit;
using MvvmCross.Platform.Logging;

namespace easyfft.iOS
{
    public class Setup : MvxIosSetup
    {
        Lazy<Core.App> app = new Lazy<Core.App>();
        public easyfft.Core.App App {
            get => app.Value;
        }
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return App;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
        Dictionary<Type, Type> AddMappedTypes()
        {
            Dictionary<Type, Type> mappedTypes = new Dictionary<Type, Type>();

            #region Adding Platform Services
            mappedTypes.Add(typeof(IAudioStream), typeof(AudioStream_iOS));
            #endregion

            return mappedTypes;
        }
        protected override IMvxIoCProvider CreateIocProvider()
        {
            App.ConfigureContainer(AddMappedTypes());
            return new Autofac.Extras.MvvmCross.AutofacMvxIocProvider( App.Container, new Autofac.Extras.MvvmCross.AutofacPropertyInjectorOptions());
        }
        protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.None;
    }
}
