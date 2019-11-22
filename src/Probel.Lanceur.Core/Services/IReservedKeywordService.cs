using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IReservedKeywordService
    {
        #region Methods

        IEnumerable<string> GetReservedKeywords();

        #endregion Methods
    }
}