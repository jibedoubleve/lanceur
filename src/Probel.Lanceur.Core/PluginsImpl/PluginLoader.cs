using Newtonsoft.Json;
using Probel.Lanceur.Infrastructure;
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

        private readonly ILogService _logger;

        #endregion Fields

        #region Constructors

        public PluginLoader(ILogService logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public IList<IPluginMetadata> LoadPlugins(string pluginRepository, Dictionary<string, Type> pluginTypes)
        {
            pluginRepository = Environment.ExpandEnvironmentVariables(pluginRepository);

            CreateIfNotExist(pluginRepository);

            var metadataList = new List<IPluginMetadata>();

            foreach (var file in Directory.EnumerateFiles(pluginRepository, "plugin.config.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);
                var metadata = JsonConvert.DeserializeObject<PluginMetadata>(json);
                metadataList.Add(metadata);

                var dir = Path.GetDirectoryName(file);
                Load(dir, metadata.Dll, pluginTypes);
            }
            return metadataList;
        }

        private void CreateIfNotExist(string dir)
        {
            if (Directory.Exists(dir) == false) { Directory.CreateDirectory(dir); }
        }

        private void Load(string dir, string dll, Dictionary<string, Type> pluginTypes)
        {
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

        #endregion Methods
    }
}