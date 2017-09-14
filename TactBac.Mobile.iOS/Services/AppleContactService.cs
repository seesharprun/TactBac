using Contacts;
using Foundation;
using System;
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
            List<Contact> contacts = new List<Contact>();
            CNContact[] contactList = new CNContact[] { };
            var keysTOFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses };

            using (var store = new CNContactStore())
            using (var predicate = CNContact.GetPredicateForContactsInContainer(store.DefaultContainerIdentifier))
            {
                NSError error;
                contactList = store.GetUnifiedContacts(predicate, keysTOFetch, out error);
            }

            foreach (var item in contactList.Where(c => c.EmailAddresses.Any()))
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
        }
    }
}