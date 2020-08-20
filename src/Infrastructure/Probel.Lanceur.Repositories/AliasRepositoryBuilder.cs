using Newtonsoft.Json;
using Probel.Lanceur.SharedKernel.Extensions;
using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Repositories
{
    public class AliasRepositoryBuilder : IAliasRepositoryBuilder
    {
        #region Fields

        private const string _sourcePath = @"%appdata%\probel\lanceur\repositories\";
        private readonly Version _applicationVersion;
        private readonly List<string> _disabledRepositories = new List<string>();
        private readonly ILogService _logger;

        private readonly IUserNotifyer _notifyer;
        private readonly IRepositoryContext _repositoryContext;
        private AliasRepositoryCollection _repositories = null;

        #endregion Fields

        #region Constructors

        public AliasRepositoryBuilder(ILogService logger, IRepositoryContext repositoryContext, IUserNotifyerFactory factory)
        {
            _notifyer = factory.Get();
            _applicationVersion = Assembly.GetEntryAssembly().GetName().Version;
            _repositoryContext = repositoryContext;
            _logger = logger;
        }

        #endregion Constructors

        #region Properties

        private AliasRepositoryCollection Repositories
        {
            get
            {
                if (_repositories == null) { Initialise(); }
                return _repositories;
            }
        }

        public bool IsInitialised => Repositories != null;

        #endregion Properties

        #region Methods

        private bool IsCompatible(RepositoryMetadata metadata)
        {
            var repoRef = $"{metadata.Keyword}-{metadata.Dll}-{metadata.Name}";
            var isDisabled = (from p in _disabledRepositories
                              where p == repoRef
                              select p).Count() > 0;

            if (isDisabled) { return false; }
            else if (_applicationVersion < metadata.MinimumVersion.ToVersion())
            {
                _disabledRepositories.Add(repoRef);
                var msg = $"Plugin '{metadata.Name}' is deactivated. It needs minimum version '{metadata.MinimumVersion}' to run. Application version is '{_applicationVersion}'";

                _logger.Warning(msg);
                _notifyer.NotifyWarning(msg);

                return false;
            }
            else { return true; }
        }

        private AliasRepositoryCollection Load(string dll, string keyword)
        {
            try
            {
                if (File.Exists(dll) == false) { throw new ArgumentException($"The file '{dll}' does not exist."); }
                else
                {
                    var asmPath = AssemblyName.GetAssemblyName(dll);
                    var asm = Assembly.Load(asmPath);
                    var repoTypes = (from t in asm.GetTypes()
                                     where t.IsClass
                                       && !t.IsAbstract
                                       && t.GetInterfaces().Contains(typeof(IAliasRepository))
                                     select t).ToList();

                    if (repoTypes.Count > 0)
                    {
                        _logger.Trace($"Loaded {repoTypes.Count} repositories.");

                        var result = new AliasRepositoryCollection();

                        foreach (var repo in repoTypes)
                        {
                            var repoInstance = (IAliasRepository)Activator.CreateInstance(repo);
                            repoInstance.Initialise(_repositoryContext);

                            var visitor = new AliasRepositoryVisitor(repoInstance);
                            if (visitor.TrySetKeyword(keyword) == false)
                            {
                                _logger.Warning($"Cannot set keyword '{keyword}' to repository of type '{repo}'");
                            }
                            result.Add(repoInstance);
                        }
                        return result;
                    }
                    else
                    {
                        _logger.Warning($"Didn't find any repository.");
                        return new AliasRepositoryCollection();
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var item in ex?.LoaderExceptions)
                {
                    _logger.Error(item.Message, ex);
                }
                throw new InvalidOperationException($"One or more repositories cannot be loaded. This is probably a version mismatch.", ex);
            }
            catch (InvalidOperationException ex) { throw new InvalidOperationException($"An error occured when searching 'Repository' class for dll '{dll}'", ex); }
        }

        public IAliasRepository GetSource(char? keyword) => GetSource(keyword?.ToString());

        public IAliasRepository GetSource(string keyword)
        {
            var result = (from r in Repositories.ToList()
                          where keyword.ToLower().StartsWith(r.Keyword.ToLower())
                          select r);
            return new AggregatedRepository(result);
        }

        public bool HasKeyword(char? keyword) => HasKeyword(keyword?.ToString());

        public bool HasKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) { return false; }

            var exists = (from r in Repositories.ToList()
                          where r.Keyword.ToLower() == keyword.ToLower()
                          select r).Count() > 0;
            return exists;
        }

        public void Initialise()
        {
            var dir = Environment.ExpandEnvironmentVariables(_sourcePath);
            var src = new AliasRepositoryCollection();

            if (Directory.Exists(dir) == false) { Directory.CreateDirectory(dir); }

            foreach (var file in Directory.EnumerateFiles(dir, "repository.config.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);
                var metadata = JsonConvert.DeserializeObject<RepositoryMetadata>(json);

                var path = Path.Combine(Path.GetDirectoryName(file), metadata.Dll);

                if (IsCompatible(metadata))
                {
                    src.Merge(Load(path, metadata.Keyword));
                }
            }

            _repositories = src;
        }

        public string NormaliseQuery(string query) => Repositories.NormaliseQuery(query);

        #endregion Methods
    }
}