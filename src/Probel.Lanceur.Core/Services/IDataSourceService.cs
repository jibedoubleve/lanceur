using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public partial interface IDataSourceService
    {
        #region Methods

        void Clear();

        void Create(Alias alias, IEnumerable<string> names = null);

        void Delete(Alias alias);

        IEnumerable<AliasName> GetNamesOf(Alias alias);

        IEnumerable<AliasSession> GetSessions();

        Alias GetAlias(string name);

        IEnumerable<AliasText> GetAliasNames(long sessionId);

        IEnumerable<Alias> GetAliases(long sessionId);

        void SetUsage(Alias alias);

        void SetUsage(long idAlias);

        void Update(Alias alias);

        void Update(IEnumerable<AliasName> names);

        AliasSession GetSession(long sessionId);

        void Update(AliasSession session);

        void Delete(AliasSession session);

        #endregion Methods
    }
}