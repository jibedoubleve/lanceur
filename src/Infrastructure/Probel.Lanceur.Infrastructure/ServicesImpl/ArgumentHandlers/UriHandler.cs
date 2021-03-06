﻿using System.Net;

namespace Probel.Lanceur.Infrastructure.ServicesImpl.ArgumentHandlers
{
    public class UriHandler : ArgumentHandler
    {
        #region Constructors

        public UriHandler() : base(Wildcards.Url)
        {
        }

        #endregion Constructors

        #region Methods

        protected override string DoHandle(string cmdline, string parameters)
        {
            var p = WebUtility.UrlEncode(parameters);
            var r = cmdline.ToLower().Replace(Wildcard, p);
            return r;
        }

        #endregion Methods
    }
}