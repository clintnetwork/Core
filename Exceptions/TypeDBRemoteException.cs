using System;
using System.Runtime.Serialization;

namespace TypeDB
{
    [Serializable]
    internal class TypeDBRemoteException : Exception
    {
        public TypeDBRemoteException()
        {
        }

        public TypeDBRemoteException(string message) : base(message)
        {
        }

        public TypeDBRemoteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeDBRemoteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}