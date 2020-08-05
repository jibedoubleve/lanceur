using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Probel.Lanceur.Plugin
{
    [DebuggerDisplay("{Command} - {Parameters}")]
    public class Cmdline
    {
        #region Constructors

        public Cmdline(Cmdline src)
        {
            Command = src.Command;
            Arguments = src.Arguments;
        }

        public Cmdline(string cmd, string param)
        {
            Command = cmd;
            Arguments = param;
        }

        #endregion Constructors

        #region Properties

        public string Command { get; private set; }
        public string Arguments { get; private set; }

        public IEnumerable<string> SplitedParameters => Arguments.Split(' ');

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get a string that represents the commandline as if it was written in the application
        /// </summary>
        /// <returns>A <see cref="string"/> representing the command line</returns>
        public override string ToString() => $"{Command} {Arguments}";

        #endregion Methods
    }
}