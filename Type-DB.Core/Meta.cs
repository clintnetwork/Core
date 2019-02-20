using Newtonsoft.Json;
using System;

namespace TypeDB
{
    [Serializable]
    public class Meta
    {
        /// <summary>
        /// Object creation date/time
        /// </summary>
        public DateTime OnCreated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Object edition date/time
        /// </summary>
        public DateTime? OnEdited { get; set; }

        /// <summary>
        /// Define if the Object has an expiration
        /// </summary>
        public TimeSpan? Expiration { get; set; }

        /// <summary>
        /// Define if the object is edited
        /// </summary>
        public bool IsUpdated { get; set; }
        
        /// <summary>
        /// Define if the object is dirty, has changes, but is not yet persisted.
        /// </summary>
        [JsonIgnore]
        public bool IsDirty { get; set; }

        /// <summary>
        /// Define if the Object is locked, the default is false
        /// </summary>
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// Define if the Object is signed, the default is false
        /// </summary>
        public bool IsSigned { get; set; } = false;

        internal string Signature { get; set; }

        /// <summary>
        /// Store the Object Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Object Version for the Update() method or other
        /// </summary>
        public int Version { get; set; } = 0;

        /// <summary>
        /// Optional Data field
        /// </summary>
        public object ExtraData { get; set; }
    }
}
