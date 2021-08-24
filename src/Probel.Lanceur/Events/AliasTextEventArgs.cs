using Probel.Lanceur.Core.Entities;
using System;

namespace Probel.Lanceur.Events
{
    public class AliasTextEventArgs : EventArgs
    {
        #region Constructors

        public AliasTextEventArgs(Query alias)
        {
            Query = alias;
        }

        #endregion Constructors

        #region Properties

        public Query Query { get; }

        #endregion Properties
    }
}