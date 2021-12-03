using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure.Utils;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Repositories;
using Probel.Lanceur.SharedKernel.Logs;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Probel.Lanceur.Infrastructure.ServicesImpl
{
    public class AliasService : IAliasService
    {
        #region Fields

        private readonly IAliasRepositoryBuilder _aliasRepositoryBuilder;
        private readonly ICommandRunner _cmdRunner;
        private readonly IDataSourceService _databaseService;
        private readonly IKeywordService _keywordService;
        private readonly IMacroRunner _macroRunner;
        private readonly IPluginManager _pluginManager;
        private readonly IParameterResolver _resolver;

        #endregion Fields

        #region Constructors

        public AliasService(IDataSourceService databaseService,
            IParameterResolver argumentHandler,
            ICommandRunner runner,
            IMacroRunner macroService,
            IPluginManager pluginManager,
            IKeywordService keywordService,
            IAliasRepositoryBuilder aliasRepositoryBuilder
            )
        {
            _aliasRepositoryBuilder = aliasRepositoryBuilder;
            _keywordService = keywordService;
            _pluginManager = pluginManager;
            _macroRunner = macroService;
            _cmdRunner = runner;

            _databaseService = databaseService;
            _resolver = argumentHandler;
        }

        #endregion Constructors

        #region Methods

        private IEnumerable<Query> LoadAliasNames(long sessionId, string keyword)
        {
            if (_aliasRepositoryBuilder.IsInitialised == false) { _aliasRepositoryBuilder.Initialise(); }

            var result = new List<Query>();
            var keyChar = keyword?.Length > 0 ? (char?)keyword[0] : null;

            if (_aliasRepositoryBuilder.HasKeyword(keyChar) == false)
            {
                var aliases = _databaseService.GetAliasNames(sessionId);
                var reserved = _keywordService.GetKeywords();
                var plugins = _pluginManager.GetKeywords().Select(e => (Query)e);

                result.AddRange(aliases);
                result.AddRange(plugins);
                result.AddRange(reserved);
            }
            else
            {
                var src = _aliasRepositoryBuilder.GetSource(keyword);
                var criterion = _aliasRepositoryBuilder.NormaliseQuery(keyword) ?? string.Empty;

                var repo = src?.GetAliases(criterion)?.Select(e => (Query)e) ?? new List<Query>();
                result.AddRange(repo);
            }

            return result;
        }

        /// <summary>
        /// Executes the command line.
        /// </summary>
        /// <param name="cmdline">The command line to execute. That's the alias and the arguments (which are not mandatory)</param>
        public ExecutionResult Execute(Query query, long idSession)
        {
            var proxy = new ExplorerProxy(query, _cmdRunner);

            if (proxy.CanOpen()) { return proxy.Open(); }
            if (_pluginManager.Exists(query.Name))
            {
                var cmd = _resolver.Split(query, idSession);
                _pluginManager.Execute(cmd);
                return ExecutionResult.SuccesShow; ;
            }
            else if (_macroRunner.Exists(query.FileName))
            {
                var a = _databaseService.GetAlias(query.Name, idSession);
                _macroRunner.Execute(a);
                return ExecutionResult.SuccessHide;
            }
            else
            {
                var a = _databaseService.GetAlias(query.Name, idSession);
                var cmd = _resolver.Split(query, idSession);
                a.FileName = _resolver.Resolve(a.FileName, cmd.Arguments);
                a.Arguments = _resolver.Resolve(a.Arguments, cmd.Arguments);

                if (a.IsEmpty)
                {
                    a.FileName = query.IsPackaged ? query.GetUniqueIdentifiyerTemplate() : query.FileName;
                }
                return _cmdRunner.Execute(a);
            }
        }

        public IEnumerable<Query> GetAliasNames(long sessionId, string criterion)
        {
            var splited = _resolver.Split(criterion, sessionId);

            var query = _aliasRepositoryBuilder.NormaliseQuery(splited.Command);

            var aliases = LoadAliasNames(sessionId, criterion);

            var result = (from a in aliases
                          where a.NameLowercase.StartsWith(query)
                          orderby a.SearchScore descending,
                                  a.ExecutionCount descending,
                                  a.Name ascending
                          select a).ToList();

            if (result.Count == 0)
            {
                /* If I'm here, it means no reslut was found. At last resort, let's try
                 * this criterion to the command line (cmd.exe)
                 */
                result.Add(Query.FromTextToCommandLine(criterion));
            }

            /* When the query is the same as a keyword, the exact match is
             * bubbled up to the first index and the rest of the result is
             * ordred normally. The goal is when the query entered is an exact
             * match and the user press <ENTER>, this alias should be executed
             */
            var tmp = (from a in aliases
                       where a.NameLowercase == query
                       select a).FirstOrDefault();
            if (tmp != null)
            {
                result.Remove(tmp);
                result.Insert(0, tmp);
            }

            return result;
        }

        public string GetSession(long id) => _databaseService.GetSession(id)?.Name ?? string.Empty;

        #endregion Methods
    }
}