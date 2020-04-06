using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public partial interface IDataSourceService
    {
        #region Methods

        void Clear();

        void Create(Alias alias, IEnumerable<string> names = null);

        void Delete(Alias alias);

        void Delete(AliasSession session);

        Alias GetAlias(string name, long sessionId);

        IEnumerable<Alias> GetAliases(long sessionId);

        IEnumerable<AliasText> GetAliasNames(long sessionId);

        IEnumerable<AliasName> GetNamesOf(Alias alias);

        AliasSession GetSession(long sessionId);

        AliasSession GetSession(string sessionName);

        IEnumerable<AliasSession> GetSessions();

        void SetUsage(Alias alias);

        void Update(Alias alias);

        void Update(IEnumerable<AliasName> names);

        void Update(AliasSession session);

        #endregion Methods
    }
}