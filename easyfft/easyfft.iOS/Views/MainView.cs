using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using easyfft.Core.ViewModels;

namespace easyfft.iOS.Views
{
    [MvxFromStoryboard]
    public partial class MainView : MvxViewController
    {
        public MainView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Apply();
        }
    }
}
