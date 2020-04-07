using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
	public partial interface IDataSourceService
	{
		#region Methods

		Alias GetAlias(string name, long sessionId);

		IEnumerable<Alias> GetAliases(long sessionId);

		IEnumerable<AliasText> GetAliasNames(long sessionId);

		IEnumerable<Doubloon> GetDoubloons(long sessionId);

		IEnumerable<AliasName> GetNamesOf(Alias alias);

		AliasSession GetSession(long sessionId);

		AliasSession GetSession(string sessionName);

		IEnumerable<AliasSession> GetSessions();

		#endregion Methods
	}
}