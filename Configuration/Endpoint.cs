using System;

namespace TypeDB
{
    public class Endpoint
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public Endpoint()
        {
            this.Host = "localhost";
            this.Port = 777;
        }

        public Endpoint(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public Uri Build()
        {
            return new UriBuilder()
            {
                Host = this.Host,
                Port = this.Port,
                Path = "/api"
            }.Uri;
        }
    }
}
