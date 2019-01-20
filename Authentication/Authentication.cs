using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TypeDB
{
    public class Authentication
    {
        public AuthenticationType Type { get; set; } = AuthenticationType.Anonymous;
        public NetworkCredential Credentials { get; set; }
    }
}
