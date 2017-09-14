using Contacts;
using Foundation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity;
using System;
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
            _eventAggregator.GetEvent<StartButtonStatus>().Publish(false);
            
            LoadApplication(application);

            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);

            using (var store = new CNContactStore())
            {
                store.RequestAccess(CNEntityType.Contacts, OnRequestPermissionsResult);
            }
        }

        public void OnRequestPermissionsResult(bool granted, NSError error)
        {
            if (granted)
            {
                _eventAggregator.GetEvent<StartButtonStatus>().Publish(true);
            }
            else
            {
                this.BeginInvokeOnMainThread(() =>
                {
                    new UIAlertView(
                        "Contact Permissions", 
                        "Contact permissions are required to use this app. To enable this permission: close this app, open the Settings app, select the Privacy option, select the Contacts option, and then enable the Contacts permission for the TactBac app.", 
                        default(IUIAlertViewDelegate), 
                        "Ok"
                    ).Show();
                });
            }
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
