using System;

namespace TypeDB.Interception
{
    [Serializable]
    internal class DTO
    {
        public string Collection { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}