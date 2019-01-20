using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Configuration Object
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Define the Instance Mode, the default is Mode.Standalone
        /// </summary>
        public Mode Mode { get; set; } = Mode.Standalone;

        /// <summary>
        /// Define if it's a Server Instance
        /// </summary>
        internal bool IsBinded { get; set; } = false;

        /// <summary>
        /// Define if the Instance will be persistent, the default is false
        /// </summary>
        internal bool IsPersistent { get; set; } = false;

        /// <summary>
        /// Define if the Instance is Sealed
        /// </summary>
        internal bool IsSealed { get; set; } = false;

        /// <summary>
        /// Define the Endpoint, it's only used if the mode is Mode.Remote
        /// </summary>
        public IPEndPoint Endpoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 777);

        /// <summary>
        /// Define the Authentication parameters
        /// </summary>
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Define the Encryption level, the default is EncryptionType.None
        /// </summary>
        public EncryptionType Encryption { get; set; } = EncryptionType.None;

        /// <summary>
        /// Define if the Instance will use SSL, the default is false
        /// </summary>
        //public bool UseSSL { get; set; } = false;

        /// <summary>
        /// Load a Configuration from a JSON File
        /// </summary>
        /// <param name="configurationFile"></param>
        /// <returns>The Configuration object loaded from the JSON file</returns>
        public static Configuration LoadFromFile(string configurationFile)
        {
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationFile));
        }

        /// <summary>
        /// Export the current Configuration in a JSON File
        /// </summary>
        /// <param name="configurationFile">The JSON filename/path</param>
        public void SaveToFile(string configurationFile)
        {
            File.WriteAllText(configurationFile, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
