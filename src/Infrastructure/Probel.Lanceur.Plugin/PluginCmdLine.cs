using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;
using System.Diagnostics;

namespace Probel.Lanceur.Plugin
{
    [DebuggerDisplay("{Command} - {Parameters}")]
    public class PluginCmdline
    {
        #region Constructors

        public PluginCmdline(PluginCmdline src)
        {
            Command = src.Command;
            Arguments = src.Arguments;
        }

        public PluginCmdline(string cmd, string param)
        {
            Command = cmd;
            Arguments = param;
        }

        #endregion Constructors

        #region Properties

        public string Arguments { get; private set; }

        public string Command { get; private set; }

        public IEnumerable<string> SplitedParameters => Arguments.Split(' ');

        #endregion Properties

        #region Methods

        public static implicit operator PluginCmdline(Cmdline cmd) => new PluginCmdline(cmd.Command, cmd.Arguments);

        /// <summary>
        /// Get a string that represents the commandline as if it was written in the application
        /// </summary>
        /// <returns>A <see cref="string"/> representing the command line</returns>
        public override string ToString() => $"{Command} {Arguments}";

        #endregion Methods
    }
}