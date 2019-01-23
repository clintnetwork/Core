using Newtonsoft.Json;
using System;
using TypeDB.Interfaces;

namespace TypeDB
{
    /// <summary>
    /// A TypeDB Object
    /// </summary>
    [Serializable]
    public class Object : IObject
    {
        // TODO: to remove
        private string _value;

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
        /// Object's Value, serialised before writed
        /// </summary>
        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = JsonConvert.SerializeObject(value);
            }
        }
    }
}
