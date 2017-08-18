using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using TactBac.Mobile.Models;
using TactBac.Mobile.Views;

namespace TactBac.Mobile.ViewModels
{
    public class FormatPageViewModel : ViewModelBase, INavigationAware, IConfirmNavigation
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private string _selectedFormatOption = "CSV";
        private Payload _payload;

        public FormatPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

        ConfirmFormatCommand = new DelegateCommand(ConfirmFormatExecute);
        }

        public DelegateCommand ConfirmFormatCommand { get; private set; }

        public List<string> FormatOptions
        {
            get
            {
                return new List<string>
                {
                    "JSON",
                    "CSV",
                    "XML"
                };
            }
        }

        public string SelectedFormatOption
        {
            get { return _selectedFormatOption; }
            set { SetProperty(ref _selectedFormatOption, value); }
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
            if (!String.IsNullOrEmpty(SelectedFormatOption))
            {
                return true;
            }
            else
            {
                _dialogService.DisplayAlertAsync(StringResources.Error_Title, StringResources.FormatPage_Error_Message, "OK");
                return false;
            }
        }

        private async void ConfirmFormatExecute()
        {
            this.IsLoading = true;

            Payload paramsPayload = _payload;
            paramsPayload.Formats = new List<string>
            {
                SelectedFormatOption
            };

            var navigationParams = new NavigationParameters();
            navigationParams.Add(nameof(Payload), paramsPayload);

            await _navigationService.NavigateAsync(nameof(DestinationPage), navigationParams);

            this.IsLoading = false;
        }
    }
}