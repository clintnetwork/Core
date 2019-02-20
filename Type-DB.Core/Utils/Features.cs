using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TypeDB.Utils
{
    internal static class Features
    {
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Make a TCP port check on a specified hostname:port
        /// </summary>
        public static bool Ping(string hostname, int port)
        {
            try
            {
                using (var client = new TcpClient(hostname, port))
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
