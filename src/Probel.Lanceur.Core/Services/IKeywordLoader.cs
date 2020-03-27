using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IKeywordLoader
    {
        #region Methods

        bool Contains(string keyword);

        IEnumerable<string> GetDefinedKeywords();

        #endregion Methods
    }
}