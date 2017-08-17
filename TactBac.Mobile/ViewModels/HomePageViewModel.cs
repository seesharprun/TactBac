using Prism.Mvvm;
using Prism.Navigation;

namespace TactBac.Mobile.ViewModels
{
    public class HomePageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public HomePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}