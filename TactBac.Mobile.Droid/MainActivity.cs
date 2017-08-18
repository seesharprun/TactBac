using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity;
using TactBac.Mobile.Droid.Services;
using TactBac.Mobile.Events;
using TactBac.Mobile.Services;

namespace TactBac.Mobile.Droid
{
    [Activity(Label = "TactBac.Mobile", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private IEventAggregator _eventAggregator;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var application = new App(new AndroidInitializer());

            var container = application.Container;
            _eventAggregator = container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<StartButtonStatus>().Publish(false);

            LoadApplication(application);
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadContacts) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadContacts }, 101);
            }
            else
            {
                _eventAggregator.GetEvent<StartButtonStatus>().Publish(true);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case 101:
                {
                    if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                    {
                        _eventAggregator.GetEvent<StartButtonStatus>().Publish(true);
                    }
                    else
                    {
                        var view = FindViewById(Android.Resource.Id.Content);
                        Snackbar.Make(view, "Contact permissions are required to use this app.", Snackbar.LengthIndefinite)
                            .SetAction("Request Permission", v => RequestPermissions(new string[] { Manifest.Permission.ReadContacts }, 101))
                            .Show();
                    }
                    return;
                }
            }
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IContactService, AndroidContactService>();
        }
    }
}

