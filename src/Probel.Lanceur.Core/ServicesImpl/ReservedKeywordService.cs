using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class ReservedKeywordService : IReservedKeywordService
    {
        #region Fields

        private IList<string> _keywords = null;

        #endregion Fields

        #region Methods

        public IEnumerable<string> GetReservedKeywords()
        {
            if (_keywords == null)
            {
                _keywords = new List<string>();
                var names = Enum.GetNames(typeof(Keywords));
                foreach (var name in names) { _keywords.Add(name.ToLower()); }
            }
            return _keywords;
        }

        #endregion Methods
    }
}