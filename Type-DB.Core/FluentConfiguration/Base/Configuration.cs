using System;
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
        #region Internal Properties
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
        #endregion

        /// <summary>
        /// Define the Instance Mode, the default is Mode.Standalone
        /// </summary>
        public Mode Mode { get; set; } = Mode.Standalone;

        /// <summary>
        /// Define the Endpoint, it's only used if the mode is Mode.Remote
        /// </summary>
        public Endpoint Endpoint { get; set; } = new Endpoint("localhost", 777);

        /// <summary>
        /// Define the Authentication parameters
        /// </summary>
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Define the Encryption level, the default is EncryptionType.None
        /// </summary>
        public EncryptionType Encryption { get; set; } = EncryptionType.None;
        
        public Persistence Persistence { get; set; } = new Persistence();

        // public Persistence Persistence { get; set; }

        // /// <summary>
        // /// Define the number of objects pr. chunk file stored on disk. If your structures are large, consider making this smaller.
        // /// This variable cannot be changed after database has been created.
        // /// </summary>
        // public int ChunkSize { get; set; }

        // /// <summary>
        // /// Define the local path where to stored all databases created in this app instance.
        // /// </summary>
        // public string AppDataPath { get; set; }

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
        /// <param name="formated">Specify if the JSON will formatted</param>
        public void SaveToFile(string configurationFile, bool formatting = true)
        {
            File.WriteAllText(configurationFile, JsonConvert.SerializeObject(this, formatting ? Formatting.Indented : Formatting.None));
        }
    }
}
