using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace TypeDB
{
    public class Configuration
    {
        public Mode Mode { get; set; } = Mode.Standalone;

        public IPEndPoint Endpoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 777);
        public Authentication Authentication { get; set; }
        public EncryptionType Encryption { get; set; } = EncryptionType.None;
        public bool IsPersistent { get; set; } = false;
        public bool UseSSL { get; set; } = false;

        public static Configuration LoadFromFile(string configurationFile)
        {
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationFile));
        }

        public void SaveToFile(string configurationFile)
        {
            File.WriteAllText(configurationFile, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
