using System;
using System.Runtime.Serialization;

namespace TypeDB.Exceptions
{
    [Serializable]
    public class TypeDBIdentifierNotFoundException : Exception
    {
        public TypeDBIdentifierNotFoundException()
        {
        }

        public TypeDBIdentifierNotFoundException(string message) : base(message)
        {
        }

        public TypeDBIdentifierNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeDBIdentifierNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}