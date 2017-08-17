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

            NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(HomePage)}");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>(nameof(NavigationPage));
            Container.RegisterTypeForNavigation<MainPage>(nameof(HomePage));
        }
    }
}