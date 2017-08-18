using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Linq;
using TactBac.Mobile.Models;
using TactBac.Mobile.Services;
using TactBac.Mobile.Views;

namespace TactBac.Mobile.ViewModels
{
    public class ListPageViewModel : ViewModelBase, IConfirmNavigation
    {
        private readonly INavigationService _navigationService;
        private readonly IContactService _contactService;
        private readonly IPageDialogService _dialogService;

        private bool _isRefreshing = false;

        public ListPageViewModel(INavigationService navigationService, IContactService contactService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _contactService = contactService;
            _dialogService = dialogService;

            Contacts = new ObservableCollection<Contact>();

            RemoveContactCommand = new DelegateCommand<Contact>(RemoveContactExecute);
            RefreshContactsCommand = new DelegateCommand(RefreshContactsExecute);
            ConfirmContactsCommand = new DelegateCommand(ConfirmContactsExecute);

            LoadContacts();
        }

        public DelegateCommand<Contact> RemoveContactCommand { get; private set; }

        public DelegateCommand RefreshContactsCommand { get; private set; }

        public DelegateCommand ConfirmContactsCommand { get; private set; }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ObservableCollection<Contact> Contacts { get; set; }
        
        public bool CanNavigate(NavigationParameters parameters)
        {
            if (Contacts.Any(c => c.Selected))
            {
                return true;
            }
            else
            { 
                _dialogService.DisplayAlertAsync(StringResources.Error_Title, StringResources.ListPage_Error_Message, "OK");
                return false;
            }
        }

        private void RemoveContactExecute(Contact parameter)
        {
            Contacts.Remove(parameter);
        }
        
        private void RefreshContactsExecute()
        {
            IsRefreshing = true;
            LoadContacts();
            IsRefreshing = false;
        }

        private async void ConfirmContactsExecute()
        {
            this.IsLoading = true;

            Payload paramsPayload = new Payload
            {
                Contacts = this.Contacts.Where(c => c.Selected).ToList()
            };

            var navigationParams = new NavigationParameters();
            navigationParams.Add(nameof(Payload), paramsPayload);

            await _navigationService.NavigateAsync(nameof(FormatPage), navigationParams);

            this.IsLoading = false;
        }

        private async void LoadContacts()
        {
            Contacts.Clear();

            IsLoading = true;

            foreach (var contact in await _contactService.GetContactNamesAsync())
            {
                contact.Selected = true;
                Contacts.Add(contact);
            }

            IsLoading = false;
        }
    }
}