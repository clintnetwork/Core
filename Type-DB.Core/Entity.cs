using Newtonsoft.Json;
using System;

namespace TypeDB
{
    /// <summary>
    /// A TypeDB Object
    /// </summary>
    [Serializable]
    public class Entity
    {
        public static string NewId()
        {
            return Guid.NewGuid().ToString();
        }

        private object _value;

        /// <summary>
        /// TypeDB Object's GUID
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// TypeDB Object's Hash
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// TypeDB Object's Namespace
        /// </summary>
        // TODO: see how to implement
        public string Namespace { get; set; } = "/";

        /// <summary>
        /// TypeDB Object's Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// TypeDB Object's Metas
        /// </summary>
        public Meta Meta { get; set; }

        [JsonProperty("Value")]
        //TODO: become internal ?
        public string ValueJson
        {
            get => JsonConvert.SerializeObject(_value);
            set => _value = JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Object's Value, serialised before writed
        /// </summary>
        [JsonIgnore]
        public object Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
