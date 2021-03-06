﻿namespace Probel.Lanceur.Infrastructure.ServicesImpl.ArgumentHandlers
{
    public class TextHandler : ArgumentHandler
    {
        #region Constructors

        public TextHandler() : base(Wildcards.Text)
        {
        }

        #endregion Constructors

        #region Methods

        protected override string DoHandle(string cmdline, string arguments)
        {
            var resolved = cmdline.ToLower().Replace(Wildcard, arguments);
            return resolved;
        }

        #endregion Methods
    }
}