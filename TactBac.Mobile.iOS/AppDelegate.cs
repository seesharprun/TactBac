using Foundation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity;
using TactBac.Mobile.Events;
using TactBac.Mobile.iOS.Services;
using TactBac.Mobile.Services;
using UIKit;

namespace TactBac.Mobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private IEventAggregator _eventAggregator;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            var application = new App(new iOSInitializer());

            var container = application.Container;
            _eventAggregator = container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<StartButtonStatus>().Publish(true);

            LoadApplication(application);

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IContactService, AppleContactService>();
        }
    }
}
