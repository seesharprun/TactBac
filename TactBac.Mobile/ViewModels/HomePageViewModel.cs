using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using TactBac.Mobile.Events;
using TactBac.Mobile.Views;

namespace TactBac.Mobile.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        private bool _startButtonEnabled;

        public HomePageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<StartButtonStatus>().Subscribe(StartButtonStatusReceived);

            StartCommand = new DelegateCommand(StartExecuteAsync);
        }

        public bool StartButtonEnabled
        {
            get { return _startButtonEnabled; }
            set { SetProperty(ref _startButtonEnabled, value); }
        }

        public DelegateCommand StartCommand { get; private set; }

        private void StartButtonStatusReceived(bool payload)
        {
            this.StartButtonEnabled = payload;
        }

        private async void StartExecuteAsync()
        {
            this.IsLoading = true;

            await _navigationService.NavigateAsync(nameof(ListPage));

            this.IsLoading = false;
        }
    }
}