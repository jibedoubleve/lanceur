using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Services
{
    public class ReservedKeywordService : IReservedKeywordService
    {
        #region Fields

        private static readonly Dictionary<string, Action<string>> _reservedKeywords = new Dictionary<string, Action<string>>();
        private static ILogService _log;

        private IList<string> _keywords = null;

        #endregion Fields

        #region Constructors

        static ReservedKeywordService()
        {
            foreach (var keyword in Enum.GetValues(typeof(Keywords)))
            {
                var key = keyword.ToString().ToUpper();
                _reservedKeywords.Add(key, arg => _log.Trace($"Action for {key}"));
            }
        }

        public ReservedKeywordService(ILogService log)
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

        public bool IsReserved(string name) => _reservedKeywords.ContainsKey(name);

        #endregion Methods
    }
}