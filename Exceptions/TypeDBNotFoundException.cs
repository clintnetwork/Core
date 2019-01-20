using System;
using System.Runtime.Serialization;

namespace TypeDB
{
    [Serializable]
    internal class TypeDBNotFoundException : Exception
    {
        public TypeDBNotFoundException()
        {
        }

        public TypeDBNotFoundException(string message) : base(message)
        {
        }

        public TypeDBNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeDBNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}