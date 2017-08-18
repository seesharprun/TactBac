using Android.Content;
using Android.Database;
using Android.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;
using TactBac.Mobile.Models;
using TactBac.Mobile.Services;
using Xamarin.Forms;

namespace TactBac.Mobile.Droid.Services
{
    public class AndroidContactService : IContactService
    {
        public async Task<IEnumerable<Contact>> GetContactNamesAsync()
        {
            return await Task<IEnumerable<Contact>>.Run(() =>
            {
                List<Contact> contactList = new List<Contact>();
                var uri = ContactsContract.CommonDataKinds.Email.ContentUri;
                string[] projection = {
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                    ContactsContract.CommonDataKinds.Email.Address
                };
                var cursor = Forms.Context.ContentResolver.Query(uri, projection, null, null, null);
                if (cursor.MoveToFirst())
                {
                    do
                    {
                        contactList.Add(
                            new Contact
                            {
                                DisplayName = cursor.GetString(cursor.GetColumnIndex(projection[1])),
                                EmailAddress = cursor.GetString(cursor.GetColumnIndex(projection[2]))
                            }
                        );
                    }
                    while (cursor.MoveToNext());
                }
                return contactList;
            });
        }
    }
}