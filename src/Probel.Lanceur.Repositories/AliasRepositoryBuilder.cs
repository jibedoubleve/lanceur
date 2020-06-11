using Newtonsoft.Json;
using Probel.Lanceur.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Repositories
{
    public class AliasRepositoryBuilder : IAliasRepositoryBuilder
    {
        #region Fields

        private const string _sourcePath = @"%appdata%\probel\lanceur\repositories\";
        private readonly ILogService _logger;

        private AliasRepositoryCollection _repositories = null;

        #endregion Fields

        #region Constructors

        public AliasRepositoryBuilder(ILogService logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Properties

        public bool IsInitialised => Repositories != null;

        private AliasRepositoryCollection Repositories
        {
            get
            {
                if (_repositories == null) { Initialise(); }
                return _repositories;
            }
        }

        #endregion Properties

        #region Methods

        public IAliasRepository GetSource(char? keyword) => GetSource(keyword?.ToString());

        public IAliasRepository GetSource(string keyword)
        {
            var result = (from r in Repositories.ToList()
                          where keyword.ToLower().StartsWith(r.Keyword.ToLower())
                          select r).FirstOrDefault();
            return result;
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

            foreach (var file in Directory.EnumerateFiles(dir, "repository.config.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);
                var metadata = JsonConvert.DeserializeObject<RepositoryMetadata>(json);

                var path = Path.Combine(Path.GetDirectoryName(file), metadata.Dll);

                src.Merge(Load(path, metadata.Keyword));
            }

            _repositories = src;
        }

        public string NormaliseQuery(string query) => Repositories.NormaliseQuery(query);

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
                            var r = (IAliasRepository)Activator.CreateInstance(repo);
                            var visitor = new AliasRepositoryVisitor(r);
                            if (visitor.TrySetKeyword(keyword) == false)
                            {
                                _logger.Warning($"Cannot set keyword '{keyword}' to repository of type '{repo}'");
                            }
                            result.Add(r);
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

        #endregion Methods
    }
}