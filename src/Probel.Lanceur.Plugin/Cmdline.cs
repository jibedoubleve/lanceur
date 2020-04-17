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
            Parameters = src.Parameters;
        }

        public Cmdline(string cmd, string param)
        {
            Command = cmd;
            Parameters = param;
        }

        #endregion Constructors

        #region Properties

        public string Command { get; private set; }
        public string Parameters { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get a string that represents the commandline as if it was written in the application
        /// </summary>
        /// <returns>A <see cref="string"/> representing the command line</returns>
        public override string ToString() => $"{Command} {Parameters}";

        #endregion Methods
    }
}