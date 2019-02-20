using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("This is the default mode, run TypDB in complete memory without backed by a TypeDB Server.")]
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
