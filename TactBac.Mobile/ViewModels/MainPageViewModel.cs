using Prism.Mvvm;

namespace TactBac.Mobile.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainPageViewModel()
        {
            this.Title = "Hello Sidney!";
        }
    }
}