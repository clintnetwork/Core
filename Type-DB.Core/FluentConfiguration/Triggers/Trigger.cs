using System;
using TypeDB.Interception;

namespace TypeDB
{
    public class Trigger
    {
        internal MethodType Method { get; }
        internal Action<DTO> Callback { get; }

        public Trigger(MethodType method, Action<DTO> callback)
        {
            Method = method;
            Callback = callback;
        }
    }
}