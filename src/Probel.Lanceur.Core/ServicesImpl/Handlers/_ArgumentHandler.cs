namespace Probel.Lanceur.Core.ServicesImpl.Handlers
{
    public abstract class ArgumentHandler
    {
        #region Constructors

        public ArgumentHandler(string wildcard) => Wildcard = wildcard.ToLower();

        #endregion Constructors

        #region Properties

        protected ArgumentHandler Next { get; private set; }
        protected string Wildcard { get; }

        protected bool HasWildCard(string txt) => txt.ToUpper() == Wildcard.ToUpper();

        #endregion Properties

        #region Methods

        protected abstract string DoHandle(string cmdline, string parameters);

        public string Handle(string cmdline, string parameters)
        {
            var result = (HasWildCard(cmdline))
                ? DoHandle(cmdline.ToLower(), parameters)
                : cmdline;

            if (Next != null) { return Next.DoHandle(result, parameters); }
            else { return result; }
        }

        public ArgumentHandler SetNext(ArgumentHandler handler)
        {
            var last = this;
            while (last.Next != null) { last = last.Next; }
            last.Next = handler;
            return this;
        }

        #endregion Methods
    }
}