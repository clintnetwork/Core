using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TypeDB.Interfaces;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Object
    /// </summary>
    [Serializable]
    public class Object : IObject
    {
        private string _value;

        public Guid Guid { get; set; } = Guid.NewGuid();

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
