using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TypeDB
{
    /// <summary>
    /// Contains all the Authentication parameters of a TypeDB Instance
    /// </summary>
    public class Authentication
    {        
        public Authentication()
        {
        }

        public Authentication(string username, string password)
        {
            this.Credentials = new NetworkCredential(username, password);
        }

        /// <summary>
        /// Authentication Type to Use
        /// </summary>
        public AuthenticationType Type { get; set; } = AuthenticationType.Anonymous;

        /// <summary>
        /// Store the credentials for a AuthenticationType.Basic Authentication
        /// </summary>
        public NetworkCredential Credentials { get; set; }
    }
}
