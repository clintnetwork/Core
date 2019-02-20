using System;

namespace TypeDB
{
    public class Endpoint
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseHttps { get; set; } = false;

        /// <summary>
        /// Create a default Endpoint
        /// </summary>
        public Endpoint()
        {
            this.Host = "localhost";
            this.Port = 777;
        }

        /// <summary>
        /// Create an Endpoint and define parameters
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useHttps"></param>
        public Endpoint(string host, int port, bool useHttps = false)
        {
            this.Host = host;
            this.Port = port;
            this.UseHttps = useHttps;
        }

        /// <summary>
        /// Build a valid Uri to query the Type-DB.Server Web API
        /// </summary>
        /// <param name="endpoint">(optional) Specify the endpoint to query</param>
        /// <param name="query">(optional) Specify a query parameter</param>
        /// <returns>Type-DB.Server endpoint Uri</returns>
        internal Uri BuildApi(string endpoint = null, string query = null)
        {
            return new UriBuilder()
            {
                Scheme = UseHttps ? "https" : "http",
                Host = this.Host,
                Port = this.Port,
                Path = endpoint ?? "/api",
                Query = query,
            }.Uri;
        }

        internal static Endpoint FromUri(Uri uri)
        {
            return new Endpoint()
            {
                Host = uri.Host,
                Port = uri.Port,
                UseHttps = uri.Scheme.Equals("https")
            };
        }
    }
}
