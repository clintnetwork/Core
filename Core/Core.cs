using System;
using System.Net;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Core Object
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Set the actual configured (or not) Instance
        /// </summary>
        internal readonly Instance CurrentInstance;

        internal Pipeline Pipeline { get; set; }

        /// <summary>
        /// Initialize TypeDB
        /// </summary>
        public Core()
        {
            this.Pipeline = new Pipeline(this);
            this.CurrentInstance = new Instance(this);
        }

        /// <summary>
        /// Initialize TypeDB and define a running Mode
        /// </summary>
        /// <param name="mode">The Mode to use, Mode.Standalone is the default value</param>
        /// <example>
        /// var tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect();
        /// </example>
        /// See <see cref="Connect()"/> to connect to the instance
        public Core(Mode mode = Mode.Standalone)
        {
            this.Pipeline = new Pipeline(this);
            this.CurrentInstance = new Instance(this)
            {
                Configuration = new Configuration
                {
                    Mode = mode
                }
            };
        }

        public Core UsePersistence()
        {
            this.CurrentInstance.Configuration.IsPersistent = true;
            return this;
        }

        public Core UseReplication()
        {
            return this;
        }

        public Core UseTriggers()
        {
            return this;
        }

        /// <summary>
        /// Configure the current Instance
        /// </summary>
        public Core Configure(Configuration configuration)
        {
            CurrentInstance.Configuration = configuration;
            return this;
        }

        /// <summary>
        /// Connect the configured Instance
        /// </summary>
        public Instance Connect()
        {
            switch ((this.CurrentInstance.Configuration ?? new Configuration()).Mode)
            {
                default:
                case Mode.Standalone:

                    return CurrentInstance;

                case Mode.Remote:

                    return CurrentInstance;
            }
        }

        /// <summary>
        /// Connect the configured Instance with a specific Connection String
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public Instance Connect(string connectionString)
        {
            if (this.CurrentInstance.Configuration.Mode == Mode.Standalone)
            {
                throw new TypeDBGeneralException("Your TypeDB instance is set to Standalone Mode, so you cannot specify an endpoint.");
            }
            return this.Connect();
        }

        /// <summary>
        /// Connect the configured Instance with a specific Connection String
        /// </summary>
        /// <param name="connectionString">Connection String as an Uri</param>
        public Instance Connect(Uri connectionString)
        {
            if (this.CurrentInstance.Configuration.Mode == Mode.Standalone)
            {
                throw new TypeDBGeneralException("Your TypeDB instance is set to Standalone Mode, so you cannot specify an endpoint.");
            }
            return this.Connect();
        }

        /// <summary>
        /// Connect the configured Instance with a specific Connection String as an IPEndPoint
        /// </summary>
        /// <param name="iPEndPoint">Connection String as an IPEndPoint</param>
        public Instance Connect(IPEndPoint iPEndPoint)
        {
            if (this.CurrentInstance.Configuration.Mode == Mode.Standalone)
            {
                throw new TypeDBGeneralException("Your TypeDB instance is set to Standalone Mode, so you cannot specify an endpoint.");
            }
            this.CurrentInstance.Configuration.Endpoint = new Endpoint()
            {
                Host = iPEndPoint.ToString(),
                Port = iPEndPoint.Port
            };
            return this.Connect();
        }

        /// <summary>
        /// Connect the configured Instance with a specific Connection String as a DnsEndPoint
        /// </summary>
        /// <param name="dnsEndPoint">Connection String as a DnsEndPoint</param>
        public Instance Connect(DnsEndPoint dnsEndPoint)
        {
            if (this.CurrentInstance.Configuration.Mode == Mode.Standalone)
            {
                throw new TypeDBGeneralException("Your TypeDB instance is set to Standalone Mode, so you cannot specify an endpoint.");
            }
            this.CurrentInstance.Configuration.Endpoint = new Endpoint()
            {
                Host = dnsEndPoint.Host,
                Port = dnsEndPoint.Port,
            };
            return this.Connect();
        }

        /// <summary>
        /// Create an Instance for TypeDB Server
        /// </summary>
        /// <remarks>This method can only be used from TypeDB Server</remarks>
        public Instance Build()
        {
            this.CurrentInstance.Configuration.IsBinded = true;
            return CurrentInstance;
        }
    }
}
