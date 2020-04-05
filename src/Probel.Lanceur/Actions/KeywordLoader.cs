using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Actions
{
    public class KeywordLoader : IKeywordLoader
    {
        #region Constructors

        public KeywordLoader(IActionCollection actions)
        {
            _actions = actions;
        }

        #endregion Constructors

        #region Properties

        public IActionCollection _actions { get; }

        public IEnumerable<ActionWord> DefinedKeywords => _actions.ToList();

        #endregion Properties

        #region Methods

        public bool Contains(string keyword)
        {
            var exists = (from k in _actions.Keywords
                          where keyword.ToLower() == k.ToLower()
                          select k).Any();
            return exists;
        }

        public IEnumerable<AliasText> GetKeywordsAsAlias()
        {
            var result = (from k in _actions.Keywords
                          select new AliasText
                          {
                              Name = k,
                              ExecutionCount = 0
                          });
            return result;
        }


        #endregion Methods
    }
}