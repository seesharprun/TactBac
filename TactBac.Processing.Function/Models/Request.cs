using System;

namespace TactBac.Processing.Function.Models
{
    public class Request
    {
        public Guid Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string FileLocation { get; set; }
    }
}