using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Net.Http;
using TactBac.Mobile.Events;
using TactBac.Mobile.Models;
using TactBac.Mobile.Views;

namespace TactBac.Mobile.ViewModels
{
    public class ConfirmationPageViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public ConfirmationPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            StartCommand = new DelegateCommand(StartExecute);
        }

        public DelegateCommand StartCommand { get; private set; }

        public void OnNavigatedFrom(NavigationParameters parameters)
        { }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            var payload = parameters.GetValue<Payload>(nameof(Payload));

            SendJob(payload);
        }

        private async void SendJob(Payload payload)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://tactbac.azurewebsites.net/api/", UriKind.Absolute);
            client.DefaultRequestHeaders.Add("x-functions-key", "ohBn3GnR2T5cF7myh9KlgDa9chLva4vUjq9r6ZR2H4Vm/pjDPxSQYw==");

            string contentJson = JsonConvert.SerializeObject(payload);

            StringContent content = new StringContent(contentJson);

            var response = await client.PostAsync(new Uri("ReceiveRequest", UriKind.Relative), content);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        { }

        private async void StartExecute()
        {
            this.IsLoading = true;

            await _navigationService.NavigateAsync(nameof(HomePage));
            _eventAggregator.GetEvent<StartButtonStatus>().Publish(true);

            this.IsLoading = false;
        }
    }
}