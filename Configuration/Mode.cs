using System;
using System.Collections.Generic;
using System.Text;

namespace TypeDB
{
    /// <summary>
    /// All TypeDB available modes
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Run TypeDB without any server
        /// </summary>
        Standalone,

        /// <summary>
        /// Connect the TypeDB Instance to a TypeDB Daemon
        /// </summary>
        Remote,

        /// <summary>
        /// Warning: this mode can only be used in a TypeDB Server
        /// </summary>
        OnlyForBinding
    }
}
