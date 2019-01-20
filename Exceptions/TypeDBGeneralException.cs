using System;
using System.Runtime.Serialization;

namespace TypeDB
{
    /// <summary>
    /// The mostly used TypeDB Exception
    /// </summary>
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