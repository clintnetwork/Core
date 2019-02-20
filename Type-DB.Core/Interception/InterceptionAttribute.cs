using System;

namespace TypeDB.Interception
{
    internal class InterceptionAttribute : Attribute
    {
        public MethodType Method { get; set; }
        // public string MethodName { get; set; }
        public InterceptionType Type { get; set; } = InterceptionType.Setter;
    }
}