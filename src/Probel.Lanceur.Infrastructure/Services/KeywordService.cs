using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Infrastructure.Services
{
    public class KeywordService : IKeywordService
    {
        #region Fields

        private static readonly Dictionary<string, Action<string>> _reservedKeywords = new Dictionary<string, Action<string>>();
        private static ILogService _log;

        #endregion Fields

        #region Constructors

        static KeywordService()
        {
            foreach (var keyword in Enum.GetValues(typeof(Keywords)))
            {
                var key = keyword.ToString().ToUpper();
                _reservedKeywords.Add(key, arg => _log.Debug($"Action for {key}"));
            }
        }

        public KeywordService(ILogService log)
        {
            _log = log;
        }

        #endregion Constructors

        #region Methods

        public void Bind(Keywords cmd, Action<string> bindedAction)
        {
            var c = cmd.ToString().ToUpper();
            if (_reservedKeywords.ContainsKey(c))
            {
                _reservedKeywords[c] = bindedAction;
            }
        }

        public void ExecuteActionFor(string name, string arg)
        {
            if (_reservedKeywords.ContainsKey(name))
            {
                var action = _reservedKeywords[name];
                action(arg);
            }
        }

        public bool IsReserved(string name) => _reservedKeywords.ContainsKey(name);

        #endregion Methods
    }
}