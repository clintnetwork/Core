using System;
using System.Collections.Generic;
using System.Text;
using TypeDB.Interfaces;

namespace TypeDB
{
    public class Meta : IMeta
    {
        public DateTime OnCreated { get; set; } = DateTime.Now;
        public DateTime OnEdited { get; set; }

        public DateTime Expiration { get; set; } = DateTime.MaxValue;

        public Type Type { get; set; }

        public int Version { get; set; } = 0;

        public object Data { get; set; }
    }
}
