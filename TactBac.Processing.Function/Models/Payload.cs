using System;
using System.Collections.Generic;

namespace TactBac.Processing.Function.Models
{
    public class Payload
    {
        public Guid Id { get; set; }

        public List<Contact> Contacts { get; set; }

        public List<string> Formats { get; set; }

        public List<string> Email { get; set; }
    }
}