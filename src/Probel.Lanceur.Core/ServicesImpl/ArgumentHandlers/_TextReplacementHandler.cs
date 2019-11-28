using System.Collections.Generic;
using System.Text;

namespace Probel.Lanceur.Core.ServicesImpl.ArgumentHandlers
{
    public class TextReplacementHandler : ArgumentHandler
    {
        #region Constructors

        public TextReplacementHandler(string wildcard) : base(wildcard)
        {
            Replacements = new Dictionary<string, string>();
        }

        #endregion Constructors

        #region Properties

        protected Dictionary<string, string> Replacements { get; }

        #endregion Properties

        #region Methods

        protected override string DoHandle(string cmdline, string arguments)
        {
            var sb = new StringBuilder(arguments);
            foreach (var item in Replacements) { sb = sb.Replace(item.Key, item.Value); }
            var normalised = sb.ToString();
            
            cmdline = cmdline.Trim().Replace(Wildcard, normalised);
            return cmdline;
        }

        #endregion Methods
    }
}