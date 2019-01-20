using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using TypeDB.Interfaces;

namespace TypeDB
{
    [Serializable]
    public class Object : IObject
    {
        private string _value;

        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Namespace { get; set; } = "/";
        public string Key { get; set; }

        public Meta Meta { get; set; }

        public bool IsLocked { get; set; } = false;
        public bool IsSigned { get; set; } = false;

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
