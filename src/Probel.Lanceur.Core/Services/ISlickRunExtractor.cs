using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface ISlickRunExtractor
    {
        #region Methods

        IEnumerable<MultiNameAlias> Extract(string fileName = null);

        #endregion Methods
    }
}