using System;
using TypeDB.Interfaces;

namespace TypeDB
{
    [Serializable]
    public class Meta : IMeta
    {
        /// <summary>
        /// Object creation date/time
        /// </summary>
        public DateTime OnCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// Object edition date/time
        /// </summary>
        public DateTime OnEdited { get; set; }

        /// <summary>
        /// Define if the Object has an expiration
        /// </summary>
        public DateTime Expiration { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// Define if the object is edited
        /// </summary>
        public bool IsUpdated { get; set; }

        /// <summary>
        /// Store the Object Type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Object Version for the Update() method or other
        /// </summary>
        public int Version { get; set; } = 0;

        /// <summary>
        /// Optional Data field
        /// </summary>
        public object Data { get; set; }
    }
}
