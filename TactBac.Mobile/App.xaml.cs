using Prism.Unity;
using TactBac.Mobile.Views;
using Xamarin.Forms;

namespace TactBac.Mobile
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) 
            : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
        }
    }
}