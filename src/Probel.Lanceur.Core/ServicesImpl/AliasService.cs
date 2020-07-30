using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        private ExecutionResult ExecuteUwp(AliasText alias)
        {
            try
            {
                const string noArgs = "";
                const ApplicationActivationHelper.ActivateOptions noFlags = ApplicationActivationHelper.ActivateOptions.None;

                var manager = new ApplicationActivationHelper.ApplicationActivationManager();
                manager.ActivateApplication(alias.UniqueIdentifier, noArgs, noFlags, out var processId);
                return ExecutionResult.SuccessHide;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                return ExecutionResult.Failure(ex.Message);
            }
        }

        private ExecutionResult ExecuteWin32(AliasText alias)
        {
            try
            {
                var a = (alias.Id == 0)
                    ? new Alias { FileName = alias.FileName }
                    : _databaseService.GetAlias(alias.Id);


                var psInfo = GetProcessStartInfo(a);
                using (var ps = new Process { StartInfo = psInfo })
                {
                    ps.Start();
                    return ExecutionResult.SuccessHide;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                return ExecutionResult.Failure(ex.Message);
            }
        }

        private ProcessStartInfo GetProcessStartInfo(Alias alias)
        {
            _log.Debug($"Executing '{alias.FileName}' with args '{alias.Arguments}'");

            var psInfo = new ProcessStartInfo()
            {
                Arguments = alias.Arguments,
                WindowStyle = alias.StartMode.AsWindowsStyle(),
                FileName = alias.FileName,
                WorkingDirectory = alias.WorkingDirectory,
            };
            if (alias.RunAs == RunAs.Admin) { psInfo.Verb = "runas"; }
            return psInfo;
        }

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
        public ExecutionResult Execute(string cmdline, long sessionId)
        {
            var cmd = _resolver.Split(cmdline, sessionId);

            var alias = _databaseService.GetAlias(cmd.Command, sessionId);
            alias = _resolver.Resolve(alias, cmd.Parameters);

            if (_pluginManager.Exists(alias.Name))
            {
                _pluginManager.Execute(cmd);
                return ExecutionResult.SuccesShow; ;
            }
            else if (_macroRunner.Exists(alias.FileName))
            {
                _macroRunner.Execute(alias);
                return ExecutionResult.SuccessHide;
            }
            else
            {
                return _cmdRunner.Execute(alias);
            }
        }

        public ExecutionResult Execute(AliasText alias)
        {
            return (alias.IsPackaged)
                ? ExecuteUwp(alias)
                : ExecuteWin32(alias);
        }

        public IEnumerable<AliasText> GetAliasNames(long sessionId, string criterion)
        {
            var splited = _resolver.Split(criterion, sessionId);

            //TODO: !! this code is called twice. To be refactored!
            var query = _aliasRepositoryBuilder.NormaliseQuery(splited.Command);

            var result = (from a in LoadAliasNames(sessionId, criterion)
                          where a.Name.ToLower().StartsWith(query)
                          select a).ToList();
            return result;
        }

        public string GetSession(long id) => _databaseService.GetSession(id)?.Name ?? string.Empty;

        #endregion Methods
    }
}