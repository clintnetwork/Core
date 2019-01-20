using System;
using System.Runtime.Serialization;

namespace TypeDB
{
    [Serializable]
    internal class TypeDBGeneralException : Exception
    {
        public TypeDBGeneralException()
        {
        }

        public TypeDBGeneralException(string message) : base(message)
        {
        }

        public TypeDBGeneralException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeDBGeneralException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}