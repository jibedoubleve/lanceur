namespace Probel.Lanceur.Core.ServicesImpl.ArgumentHandlers
{
    public class TextHandler : ArgumentHandler
    {
        public TextHandler() : base(Wildcards.Text)
        {
        }
        #region Methods

        protected override string DoHandle(string cmdline, string arguments)
        {
            var resolved = cmdline.Replace(Wildcard, arguments);
            return resolved;
        }

        #endregion Methods
    }
}