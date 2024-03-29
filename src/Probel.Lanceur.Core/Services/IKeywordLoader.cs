﻿using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IKeywordLoader
    {
        #region Methods

        bool Contains(string keyword);

        IEnumerable<ActionWord> DefinedKeywords { get; }

        IEnumerable<Query> GetKeywordsAsAlias();

        #endregion Methods
    }
}