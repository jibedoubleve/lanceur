﻿using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Infrastructure.ServicesImpl
{
    public class KeywordService : IKeywordService
    {
        #region Fields

        private static readonly Dictionary<string, Func<string, ExecutionResult>> _reservedKeywords = new Dictionary<string, Func<string, ExecutionResult>>();

        private readonly IKeywordLoader _keywordLoader;

        #endregion Fields

        #region Constructors

        public KeywordService(IKeywordLoader keywordLoader)
        {
            _keywordLoader = keywordLoader;
        }

        #endregion Constructors

        #region Methods

        public void Bind(string keyword, Func<string, ExecutionResult> bindedAction)
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
        public ExecutionResult ExecuteActionFor(string name, string arg)
        {
            if (IsReserved(name))
            {
                var action = _reservedKeywords[name];
                return action(arg);
            }
            else { return ExecutionResult.None; }
        }

        public IEnumerable<Query> GetKeywords()
        {
            var r = from k in _keywordLoader.DefinedKeywords
                    select Query.ReservedKeyword(k);
            return r;
        }

        public bool IsReserved(string name) => _reservedKeywords.ContainsKey(name);

        #endregion Methods
    }
}