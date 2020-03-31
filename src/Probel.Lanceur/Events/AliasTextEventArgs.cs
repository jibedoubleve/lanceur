using Probel.Lanceur.Core.Entities;
using System;

namespace Probel.Lanceur.Events
{
    public class AliasTextEventArgs : EventArgs
    {
        #region Constructors

        public AliasTextEventArgs(AliasText alias)
        {
            Alias = alias;
        }

        #endregion Constructors

        #region Properties

        public AliasText Alias { get; }

        #endregion Properties
    }
}