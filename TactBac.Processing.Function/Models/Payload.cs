using System;
using System.Collections.Generic;

namespace TactBac.Processing.Function.Models
{
    public class Payload
    {
        public List<Contact> Contacts { get; set; }

        public List<Format> Formats { get; set; }

        public List<string> Email { get; set; }
    }
}