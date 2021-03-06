﻿using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public partial interface IDataSourceService
    {
        #region Methods

        void Clear();

        void Create(Alias alias, IEnumerable<string> names = null);

        void Create(AliasSession session);

        void Delete(Alias alias);

        void Delete(AliasSession session);

        void SetUsage(Alias alias);

        void Update(Alias alias, IEnumerable<string> names = null);

        //void UpdateNames(IEnumerable<string> names, long idAlias);

        void Update(AliasSession session);

        #endregion Methods
    }
}