using Android.Content;
using System;
using System.Collections.Generic;
using easyfft.Core.Services;
using easyfft.Droid.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Logging;

namespace easyfft.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        Lazy<Core.App> app = new Lazy<Core.App>();
        public easyfft.Core.App App
        {
            get => app.Value;
        }
        public Setup(Context applicationContext) : base(applicationContext)
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
        Dictionary<Type, Type> GetMappedTypes()
        {
            Dictionary<Type, Type> mappedTypes = new Dictionary<Type, Type>();

            #region Adding Platform Services
            mappedTypes.Add(typeof(IAudioStream), typeof(AudioStream_Android));
            #endregion

            return mappedTypes;
        }
        protected override IMvxIoCProvider CreateIocProvider()
        {
            App.ConfigureContainer(GetMappedTypes());
            return new Autofac.Extras.MvvmCross.AutofacMvxIocProvider(App.Container, new Autofac.Extras.MvvmCross.AutofacPropertyInjectorOptions());
        }
        protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.None;
    }
}
