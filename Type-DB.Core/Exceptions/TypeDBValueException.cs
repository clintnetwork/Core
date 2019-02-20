using System;
using System.Runtime.Serialization;

namespace TypeDB.Exceptions
{
    [Serializable]
    public class TypeDBValueException : Exception
    {
        public TypeDBValueException()
        {
        }

        public TypeDBValueException(string message) : base(message)
        {
        }

        public TypeDBValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeDBValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}