using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using Foundation;
using System;
using UIKit;

namespace easyfft.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, Window);
            try
            {
                setup.Initialize();
            }
            catch(Exception e)
            {
                string s = e.Message;
            }
            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            return true;
        }
    }
}
