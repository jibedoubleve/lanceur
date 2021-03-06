﻿namespace Probel.Lanceur.Infrastructure.ServicesImpl.ArgumentHandlers
{
    public abstract class ArgumentHandler
    {
        #region Constructors

        public ArgumentHandler(string wildcard) => Wildcard = wildcard.ToLower();

        #endregion Constructors

        #region Properties

        protected ArgumentHandler Next { get; private set; }
        protected string Wildcard { get; }

        #endregion Properties

        #region Methods

        protected abstract string DoHandle(string cmdline, string parameters);

        protected bool HasWildCard(string txt) => txt.ToLower().Contains(Wildcard);

        public string Handle(string text, string parameters)
        {
            if (string.IsNullOrWhiteSpace(text)) { return parameters; }
            else
            {
                var result = (HasWildCard(text))
                    ? DoHandle(text, parameters)
                    : text;

                if (Next != null) { return Next.Handle(result, parameters); }
                else { return result; }
            }
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