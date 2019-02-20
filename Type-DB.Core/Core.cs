using System;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using TypeDB.Exceptions;
using TypeDB.Tunneling;
using TypeDB.Utils;
using Microsoft.Extensions.Logging;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Core
    /// </summary>
    public class Core : IDisposable
    {
        // public ILoggerFactory loggerFactory = new LoggerFactory();
        // public ILogger<Core> _logger;

        /// <summary>
        /// Set the actual configured (or not) Instance
        /// </summary>
        internal readonly Instance CurrentInstance;

        internal Pipeline Pipeline { get; set; }

        /// <summary>
        /// Initialize TypeDB and define a running Mode
        /// </summary>
        /// <param name="mode">The Mode to use, Mode.Standalone is the default value</param>
        /// <example>
        /// var tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect();
        /// </example>
        /// See <see cref="Connect()"/> to Connect to the Instance
        public Core(Mode mode = Mode.Standalone)
        {
            // _logger = loggerFactory.CreateLogger<Core>();
            this.Pipeline = new Pipeline(this);
            this.CurrentInstance = new Instance(this)
            {
                Configuration = new Configuration
                {
                    Mode = mode
                }
            };
        }

        /// <summary>
        /// Setup Persistence for the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core UsePersistence(Persistence persistence)
        {
            // Console.WriteLine("ok");
            // _logger.LogInformation("ok");
            this.CurrentInstance.Configuration.IsPersistent = true;
            this.CurrentInstance.Configuration.Persistence = persistence;
            return this;
        }

        /// <summary>
        /// Setup Encryption for the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core UseEncryption(EncryptionType encryptionType = EncryptionType.AES)
        {
            this.CurrentInstance.Configuration.Encryption = encryptionType;
            return this;
        }

        /// <summary>
        /// Setup Authentication for the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core UseAuthentication(Authentication authentication)
        {
            this.CurrentInstance.Configuration.Authentication = authentication;
            return this;
        }

        /// <summary>
        /// Setup Authentication for the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core UseAuthentication(AuthenticationType authenticationType, NetworkCredential credentials)
        {
            this.CurrentInstance.Configuration.Authentication = new Authentication
            {
                Type = authenticationType,
                Credentials = credentials
            };
            return this;
        }

        /// <summary>
        /// Setup Replication for the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core UseReplication()
        {
            return this;
        }

        public Core UseTriggers(Trigger[] triggers)
        {
            CurrentInstance.Triggers.AddRange(triggers);
            return this;
        }

        /// <summary>
        /// Setup Triggers fo the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core UseTriggers(Trigger trigger)
        {
            CurrentInstance.Triggers.Add(trigger);
            return this;
        }

        /// <summary>
        /// Configure the Current Instance
        /// </summary>
        /// <returns>Return the Core Instance</returns>
        public Core Configure(Configuration configuration)
        {
            //TODO: fix this
            //CurrentInstance.Configuration = configuration;
            return this;
        }

        /// <summary>
        ///  Connect the configured Instance with a specific Endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint as Connection String</param>
        /// <see cref="Endpoint" />
        public Instance Connect(Endpoint endpoint)
        {
            if (this.CurrentInstance.Configuration.Mode == Mode.Standalone)
            {
                throw new TypeDBGeneralException("Your TypeDB instance is set to Standalone Mode, so you cannot specify an endpoint.");
            }
            this.CurrentInstance.Configuration.Endpoint = endpoint;
            return this.Connect();
        }

        /// <summary>
        /// Connect the configured Instance with a specific Connection String
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public Instance Connect(string connectionString)
        {
            return this.Connect(new Uri(connectionString));
        }

        /// <summary>
        /// Connect the configured Instance with a specific Connection String
        /// </summary>
        /// <param name="connectionString">Uri as Connection String</param>
        public Instance Connect(Uri connectionString)
        {
            if (this.CurrentInstance.Configuration.Mode == Mode.Standalone)
            {
                throw new TypeDBGeneralException("Your TypeDB instance is set to Standalone Mode, so you cannot specify an endpoint.");
            }
            this.CurrentInstance.Configuration.Endpoint = Endpoint.FromUri(connectionString);
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
        /// <param name="dnsEndPoint">DnsEndPoint as Connection String</param>
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
        /// Connect the configured Instance
        /// </summary>
        public Instance Connect()
        {
            //TODO: see to remove configuration check
            switch ((this.CurrentInstance.Configuration ?? new Configuration()).Mode)
            {
                default:
                case Mode.Standalone:
                    if (this.CurrentInstance.Configuration.IsPersistent)
                    {
                        var persistence = this.CurrentInstance.Configuration.Persistence;

                        // Fetch previous datas
                        persistence.Fetch(CurrentInstance);

                        // Execute a timer if the Persistence is configured to PersistenceType.Snapshot, and perform the data writing every n interval
                        if (persistence.Type == PersistenceType.Snapshot)
                        {
                            var timer = new Timer(x => persistence.Invoke(CurrentInstance), null, TimeSpan.Zero, persistence.Interval);
                        }
                    }
                    return CurrentInstance;

                case Mode.Remote:
                    if (Features.Ping(this.CurrentInstance.Configuration.Endpoint.Host, this.CurrentInstance.Configuration.Endpoint.Port) == false)
                    {
                        throw new TypeDBRemoteException($"The remote endpoint is unavailable.");
                    }
                    return CurrentInstance;
            }
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
