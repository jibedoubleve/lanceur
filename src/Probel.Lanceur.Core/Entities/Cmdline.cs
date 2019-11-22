namespace Probel.Lanceur.Core.Entities
{
    public class Cmdline
    {
        #region Constructors

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
    }
}