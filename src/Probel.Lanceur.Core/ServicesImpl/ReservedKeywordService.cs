using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class ReservedKeywordService : IReservedKeywordService
    {
        #region Fields

        private static readonly Dictionary<string, Action<string>> _reservedKeywords = new Dictionary<string, Action<string>>();
        private readonly IKeywordLoader _keywordLoader;

        #endregion Fields

        #region Constructors

        public ReservedKeywordService(ILogService log, IKeywordLoader keywordLoader)
        {
            _keywordLoader = keywordLoader;
        }

        #endregion Constructors

        #region Methods

        public void Bind(string keyword, Action<string> bindedAction)
        {
            var c = keyword.ToUpper();
            if (_reservedKeywords.ContainsKey(c))
            {
                _reservedKeywords[c] = bindedAction;
            }
            else if (_keywordLoader.Contains(keyword))
            {
                _reservedKeywords.Add(c, bindedAction);
            }
        }

        /// <summary>
        /// Executed the action attached to the binding.
        /// In a words, it handle reserved keywords.
        /// </summary>
        /// <param name="name">The name of the reserved keyword</param>
        /// <param name="arg">The arguments attached to the keyword</param>
        public void ExecuteActionFor(string name, string arg)
        {
            if (IsReserved(name))
            {
                var action = _reservedKeywords[name];
                action(arg);
            }
        }

        public IEnumerable<AliasText> GetKeywords()
        {
            var r = from k in _keywordLoader.DefinedKeywords
                    select AliasText.ReservedKeyword(k);
            return r;
        }

        public bool IsReserved(string name) => _reservedKeywords.ContainsKey(name);

        #endregion Methods
    }
}