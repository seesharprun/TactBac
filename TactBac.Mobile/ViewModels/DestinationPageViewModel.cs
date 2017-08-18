using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using TactBac.Mobile.Models;
using TactBac.Mobile.Views;

namespace TactBac.Mobile.ViewModels
{
    public class DestinationPageViewModel : ViewModelBase, INavigationAware, IConfirmNavigation
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private string _providedEmailAddress;
        private Payload _payload;

        public DestinationPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            ConfirmEmailCommand = new DelegateCommand(ConfirmEmailExecute);
        }

        public DelegateCommand ConfirmEmailCommand { get; private set; }

        public string ProvidedEmailAddress
        {
            get { return _providedEmailAddress; }
            set { SetProperty(ref _providedEmailAddress, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        { }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            _payload = parameters.GetValue<Payload>(nameof(Payload));
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        { }

        public bool CanNavigate(NavigationParameters parameters)
        {
            if(!String.IsNullOrEmpty(ProvidedEmailAddress))
            {
                return true;
            }
            else
            {
                _dialogService.DisplayAlertAsync(StringResources.Error_Title, StringResources.DestinationPage_Error_Message, "OK");
                return false;
            }
        }

        private async void ConfirmEmailExecute()
        {
            this.IsLoading = true;

            Payload paramsPayload = _payload;
            paramsPayload.Email = new List<string>
            {
                ProvidedEmailAddress
            };

            var navigationParams = new NavigationParameters();
            navigationParams.Add(nameof(Payload), paramsPayload);

            await _navigationService.NavigateAsync(nameof(ConfirmationPage), navigationParams);

            this.IsLoading = false;
        }
    }
}