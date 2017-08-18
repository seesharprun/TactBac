using System.Collections.Generic;

namespace TactBac.Mobile.Models
{
    public class Payload
    {
        public List<Contact> Contacts { get; set; }

        public List<string> Formats { get; set; }

        public List<string> Email { get; set; }
    }
}