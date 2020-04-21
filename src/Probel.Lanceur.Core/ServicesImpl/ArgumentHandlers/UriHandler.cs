using System.Net;

namespace Probel.Lanceur.Core.ServicesImpl.ArgumentHandlers
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
            var r = cmdline.Replace(Wildcard, p);
            return r;
        }

        #endregion Methods
    }
}