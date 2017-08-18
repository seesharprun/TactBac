using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Net.Http;
using TactBac.Mobile.Models;
using TactBac.Mobile.Views;

namespace TactBac.Mobile.ViewModels
{
    public class ConfirmationPageViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        public ConfirmationPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

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
            client.BaseAddress = new Uri("http://tactbac.azurewebsites.net/api/", UriKind.Absolute);
            client.DefaultRequestHeaders.Add("x-functions-key", "MnQHfHcP7k9q/zPC30x6XDtPaiOpkou3rTkes2vjU3CZasCaQ1tMZw==");

            string contentJson = JsonConvert.SerializeObject(payload);

            StringContent content = new StringContent(contentJson);

            var response = await client.PostAsync(new Uri("HttpTriggerJS1", UriKind.Relative), content);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        { }

        private async void StartExecute()
        {
            this.IsLoading = true;

            await _navigationService.NavigateAsync(nameof(HomePage));

            this.IsLoading = false;
        }
    }
}