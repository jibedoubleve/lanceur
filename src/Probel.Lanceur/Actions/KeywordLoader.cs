using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Actions
{
    public class KeywordLoader : IKeywordLoader
    {
        #region Fields

        private readonly ILogService _logger;
        private List<string> _keywords;

        #endregion Fields

        #region Constructors

        public KeywordLoader(ILogService logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public bool Contains(string keyword)
        {
            if (_keywords == null) { GetDefinedKeywords(); }

            var exists = (from k in _keywords
                          where keyword.ToLower() == k.ToLower()
                          select k).Any();
            return exists;
        }

        public IEnumerable<string> GetDefinedKeywords()
        {
            var types = from t in Assembly.GetAssembly(typeof(IUiAction)).GetTypes()
                        where t.GetCustomAttribute<UiActionAttribute>() != null
                        select t;

            if (_keywords == null)
            {
                _keywords = new List<string>();
                foreach (var type in types)
                {
                    var actionName = type.Name.Replace("Action", "").ToLower();
                    _logger.Trace($"Found type '{type.Name}' for action '{actionName}'");
                    _keywords.Add(actionName);
                }
            }
            return _keywords;
        }

        public IEnumerable<AliasText> GetKeywordsAsAlias()
        {
            var result = (from k in GetDefinedKeywords()
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