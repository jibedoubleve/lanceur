using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Plugins;
using Probel.Lanceur.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.Core.PluginsImpl
{
    public class PluginManager : IPluginManager, IPluginInfoManager
    {
        #region Fields

        private const string _pluginRepository = @"%appdata%\probel\lanceur\plugins\";
        private readonly IApplicationManager _appManager;
        private readonly ILogService _logger;
        private readonly IPluginLoader _pluginLoader;
        private readonly Dictionary<string, Type> _pluginTypes = new Dictionary<string, Type>();
        private IList<IPluginMetadata> _metadataList;

        #endregion Fields

        #region Constructors

        public PluginManager(IPluginLoader loader, ILogService logger, IApplicationManager appManager)
        {
            _appManager = appManager;
            _logger = logger;
            _pluginLoader = loader;
        }

        #endregion Constructors

        #region Methods

        public IPlugin Build(string name)
        {
            _metadataList = _pluginLoader.LoadPlugins(_pluginRepository, _pluginTypes);
            name = name.ToLower();

            var metadata = GetMetadataList(name).FirstOrDefault();

            if (metadata == null)
            {
                var p = new EmptyPlugin();
                p.Initialise(_logger, _appManager);
                return p;
            }
            else if (_pluginTypes.ContainsKey(metadata.Dll))
            {
                var type = _pluginTypes[metadata.Dll];
                var plugin = Activator.CreateInstance(type);

                var p = plugin as PluginBase;
                p?.Initialise(_logger, _appManager);
                return p;
            }
            else { throw new NotSupportedException($"Cannot instanciate the plugin '{metadata.Dll}'. Did you forget to load the plugins?"); }
        }

        public bool Exists(string name)
        {
            var exist = GetMetadataList(name.ToLower());
            if (exist.Any())
            {
                _logger.Info($"Found {exist.Count()} plugin(s) with criteria '{name}'");
                foreach (var plugin in exist) { _logger.Trace($"Plugin '{plugin.Name}' [Keyword: {plugin.Keyword}] - {plugin.Description}"); }
                return true;
            }
            else { return false; }
        }

        public IEnumerable<AliasText> GetKeywords()
        {
            var keywords = (from k in GetPluginsInfo()
                            select new AliasText
                            {
                                ExecutionCount = 0,
                                FileName = $"[{k.Name}] {k.Description}",
                                Kind = "Puzzle",
                                Name = string.IsNullOrWhiteSpace(k.Keyword) ? k.Name : k.Keyword
                            });

            return keywords;
        }

        public IEnumerable<IPluginMetadata> GetPluginsInfo()
        {
            if (_metadataList == null)
            {
                _metadataList = _pluginLoader.LoadPlugins(_pluginRepository, _pluginTypes);
            }

            return _metadataList ?? new List<IPluginMetadata>();
        }

        private IEnumerable<IPluginMetadata> GetMetadataList(string name)
        {
            var metadata = (from m in GetPluginsInfo()
                            where m.Keyword == name
                            select m);
            return metadata;
        }

        #endregion Methods
    }
}