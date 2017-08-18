using Prism.Mvvm;

namespace TactBac.Mobile.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        private bool _isLoading = false;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
    }
}