using Contacts;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TactBac.Mobile.Models;
using TactBac.Mobile.Services;

namespace TactBac.Mobile.iOS.Services
{
    public class AppleContactService : IContactService
    {
        public async Task<IEnumerable<Contact>> GetContactNamesAsync()
        {
            return await Task<IEnumerable<Contact>>.Run(() =>
            {
                List<Contact> contacts = new List<Contact>();
                NSError error;
                CNContact[] contactList;
                var keysTOFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses };
                var ContainerId = new CNContactStore().DefaultContainerIdentifier;
                using (var predicate = CNContact.GetPredicateForContactsInContainer(ContainerId))
                using (var store = new CNContactStore())
                {
                    contactList = store.GetUnifiedContacts(predicate, keysTOFetch, out error);
                }
                foreach (var item in contactList)
                {
                    if (null != item && null != item.EmailAddresses)
                    {
                        contacts.Add(new Contact
                        {
                            DisplayName = $"{item.GivenName} {item.FamilyName}",
                            EmailAddress = item.EmailAddresses.FirstOrDefault()?.Value
                        });
                    }
                }
                return contacts;
            });
        }
    }
}