using System;

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
        private readonly Instance CurrentInstance;

        /// <summary>
        /// Initialize TypeDB
        /// </summary>
        public Core()
        {
            this.CurrentInstance = new Instance();
        }

        /// <summary>
        /// Initialize TypeDB and define a running Mode
        /// </summary>
        /// <param name="mode">The Mode to use</param>
        public Core(Mode mode)
        {
            this.CurrentInstance = new Instance()
            {
                Configuration = new Configuration()
                {
                    Mode = mode
                }
            };
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
            switch ((this.CurrentInstance.Configuration??new Configuration()).Mode)
            {
                default:
                case Mode.Standalone:
                    return CurrentInstance;

                case Mode.Remote:
                    /*if (string.IsNullOrEmpty(this.CurrentInstance.Configuration.Host) || string.IsNullOrEmpty(CurrentInstance.Configuration.Port.ToString()))
                    {
                        throw new TypeDBRemoteException("On the Remote Mode, you need to configure the endpoint.");
                    }*/
                    /*if (this.Configuration == null)
                    {
                        throw new TypeDBGeneralException("You need to configure the TypeDB Instance before to connect.");
                    }*/
                    return null;
            }
        }

        /// <summary>
        /// Create an Instance for Type-DB Server
        /// </summary>
        public Instance Bind()
        {
            return CurrentInstance;
        }
    }
}
