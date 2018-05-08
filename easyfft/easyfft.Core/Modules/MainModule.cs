using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core;
using MvvmCross.Core;

namespace easyfft.Core.Modules
{
    class MainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewModels.MainViewModel>().SingleInstance();
        }
    }
}
