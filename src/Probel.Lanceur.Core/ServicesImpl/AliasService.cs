﻿using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Repositories;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class AliasService : IAliasService
    {
        #region Fields

        private readonly IAliasRepositoryBuilder _aliasRepositoryBuilder;
        private readonly ICommandRunner _cmdRunner;
        private readonly IDataSourceService _databaseService;
        private readonly IKeywordService _keywordService;
        private readonly ILogService _log;
        private readonly IMacroRunner _macroRunner;
        private readonly IPluginManager _pluginManager;
        private readonly IParameterResolver _resolver;

        #endregion Fields

        #region Constructors

        public AliasService(IDataSourceService databaseService,
            IParameterResolver argumentHandler,
            ICommandRunner runner,
            ILogService log,
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

            _log = log;
            _databaseService = databaseService;
            _resolver = argumentHandler;
        }

        #endregion Constructors

        #region Methods

        private IEnumerable<AliasText> LoadAliasNames(long sessionId, string keyword)
        {
            if (_aliasRepositoryBuilder.IsInitialised == false) { _aliasRepositoryBuilder.Initialise(); }

            var result = new List<AliasText>();
            var keyChar = keyword?.Length > 0 ? (char?)keyword[0] : null;

            if (_aliasRepositoryBuilder.HasKeyword(keyChar) == false)
            {
                var aliases = _databaseService.GetAliasNames(sessionId);
                var reserved = _keywordService.GetKeywords();
                var plugins = _pluginManager.GetKeywords().Select(e => (AliasText)e);

                result.AddRange(aliases);
                result.AddRange(plugins);
                result.AddRange(reserved);
            }
            else
            {
                var src = _aliasRepositoryBuilder.GetSource(keyword);
                var criterion = _aliasRepositoryBuilder.NormaliseQuery(keyword) ?? string.Empty;

                var repo = src?.GetAliases(criterion)?.Select(e => (AliasText)e) ?? new List<AliasText>();
                result.AddRange(repo);
            }

            return result;
        }

        /// <summary>
        /// Executes the command line.
        /// </summary>
        /// <param name="cmdline">The command line to execute. That's the alias and the arguments (which are not mandatory)</param>
        public ExecutionResult Execute(AliasText alias, string cmdline, long idSession)
        {
            if (_pluginManager.Exists(alias.Name))
            {
                var cmd = _resolver.Split(cmdline, idSession);
                _pluginManager.Execute(cmd);
                return ExecutionResult.SuccesShow; ;
            }
            else if (_macroRunner.Exists(alias.FileName))
            {
                var a = _databaseService.GetAlias(alias.Name, idSession);
                _macroRunner.Execute(a);
                return ExecutionResult.SuccessHide;
            }
            else
            {
                var a = _databaseService.GetAlias(alias.Name, idSession);

                if (a.IsEmpty)
                {
                    a.FileName = alias.IsPackaged ? alias.GetUniqueIdentifiyerTemplate() : alias.FileName;
                    return _cmdRunner.Execute(a);
                }
                else
                {
                    var cmd = _resolver.Split(cmdline, idSession);
                    a.FileName = _resolver.Resolve(a.FileName, cmd.Arguments);
                    a.Arguments = _resolver.Resolve(a.Arguments, cmd.Arguments);

                    return _cmdRunner.Execute(a);
                }
            }
        }

        public IEnumerable<AliasText> GetAliasNames(long sessionId, string criterion)
        {
            var splited = _resolver.Split(criterion, sessionId);

            var query = _aliasRepositoryBuilder.NormaliseQuery(splited.Command);

            var aliases = LoadAliasNames(sessionId, criterion);
            IEnumerable<AliasText> result = null;

            result = (from a in aliases
                      where a.NameLowercase == query
                      select a).ToList();

            if (result.Count() == 1) { return result; }
            else
            {

                result = (from a in aliases
                          where a.NameLowercase.StartsWith(query)
                          orderby a.SearchScore descending,
                                  a.ExecutionCount descending,
                                  a.Name ascending
                          select a).ToList();
                return result;
            }
        }

        public string GetSession(long id) => _databaseService.GetSession(id)?.Name ?? string.Empty;

        #endregion Methods
    }
}