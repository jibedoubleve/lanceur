using Newtonsoft.Json;
using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Infrastructure.Helpers;
using Probel.Lanceur.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Core.PluginsImpl
{
    public class PluginLoader : IPluginLoader
    {
        #region Fields

        private readonly Version _applicationVersion;
        private readonly ILogService _logger;
        private readonly IUserNotifyer _notifyer;
        private List<string> _disabledPlugins = new List<string>();

        #endregion Fields

        #region Constructors

        public PluginLoader(ILogService logger, IUserNotifyerFactory factory)
        {
            _notifyer = factory.Get();
            _logger = logger;
            _applicationVersion = Assembly.GetEntryAssembly().GetName().Version;
        }

        #endregion Constructors

        #region Methods

        private void CreateDirIfNotExist(string dir)
        {
            if (Directory.Exists(dir) == false) { Directory.CreateDirectory(dir); }
        }

        private bool IsCompatible(PluginMetadata metadata)
        {
            var pluginRef = $"{metadata.Description}-{metadata.Name}-{metadata.Dll}";
            var isDisabled = (from p in _disabledPlugins
                              where p == pluginRef
                              select p).Count() > 0;

            if (isDisabled) { return false; }
            else if (_applicationVersion < metadata.MinimumVersion.ToVersion())
            {
                _disabledPlugins.Add(pluginRef);
                var msg = $"Plugin '{metadata.Name}' is deactivated. It needs minimum version '{metadata.MinimumVersion}' to run. Application version is '{_applicationVersion}'";

                _logger.Warning(msg);
                _notifyer.NotifyWarning(msg);

                return false;
            }
            else { return true; }
        }

        private void Load(string dir, PluginMetadata metadata, Dictionary<string, Type> pluginTypes)
        {
            var dll = metadata.Dll;

            var path = Path.Combine(dir, dll);
            if (File.Exists(path) == false) { throw new ArgumentException($"Cannot load plugin's file '{path}', file does not exist."); }
            else if (pluginTypes.ContainsKey(dll) == false)
            {
                try
                {
                    var asmPath = AssemblyName.GetAssemblyName(path);
                    var assembly = Assembly.Load(asmPath);
                    var type = (from t in assembly.GetTypes()
                                where t.IsClass
                                   && !t.IsAbstract
                                   && t.GetInterfaces().Contains(typeof(IPlugin))
                                select t).FirstOrDefault();

                    if (type != null)
                    {
                        _logger.Trace($"Loading plugin '{type}' from dll '{dll}'");
                        pluginTypes.Add(dll, type);
                    }
                    else { _logger.Warning($"Didn't find any plugin."); }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var msg = string.Empty;
                    foreach (var item in ex?.LoaderExceptions)
                    {
                        _logger.Error(item.Message, ex);
                    }
                    throw new InvalidOperationException($"One or more plugins cannot be loaded. This is probably a version mismatch.", ex);
                }
                catch (InvalidOperationException ex) { throw new InvalidOperationException($"An error occured when searching 'Plugin' class for dll '{dll}'", ex); }
            }
        }

        public IList<IPluginMetadata> LoadPlugins(string pluginRepository, Dictionary<string, Type> pluginTypes)
        {
            pluginRepository = Environment.ExpandEnvironmentVariables(pluginRepository);

            CreateDirIfNotExist(pluginRepository);

            var metadataList = new List<IPluginMetadata>();

            foreach (var file in Directory.EnumerateFiles(pluginRepository, "plugin.config.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);
                var metadata = JsonConvert.DeserializeObject<PluginMetadata>(json);

                if (IsCompatible(metadata))
                {
                    metadataList.Add(metadata);

                    var dir = Path.GetDirectoryName(file);
                    Load(dir, metadata, pluginTypes);
                }
            }
            return metadataList;
        }

        #endregion Methods
    }
}