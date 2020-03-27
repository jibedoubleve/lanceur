using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Services
{
    public class ReservedKeywordService : IReservedKeywordService
    {
        #region Fields

        private static readonly Dictionary<string, Action<string>> _reservedKeywords = new Dictionary<string, Action<string>>();
        private readonly IKeywordLoader _keywordLoader;
        private static ILogService _log;

        private IList<string> _keywords = null;

        #endregion Fields

        #region Constructors

        public ReservedKeywordService(ILogService log, IKeywordLoader keywordLoader)
        {
            _keywordLoader = keywordLoader;
           _log = log;
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
            else if(_keywordLoader.Contains(keyword))
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

        public IEnumerable<string> GetReservedKeywords() => _keywordLoader.GetDefinedKeywords();

        public bool IsReserved(string name) => _reservedKeywords.ContainsKey(name);

        #endregion Methods
    }
}