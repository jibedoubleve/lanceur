using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Core.Plugins
{
    public class PluginLoader : IPluginLoader
    {
        #region Methods

        public IList<IPluginMetadata> LoadPlugins(string pluginRepository, Dictionary<string, Type> pluginTypes)
        {
            pluginRepository = Environment.ExpandEnvironmentVariables(pluginRepository);
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
                                select t).First();

                    pluginTypes.Add(dll, type);
                }
                catch (InvalidOperationException ex) { throw new InvalidOperationException($"An error occured when searching 'Plugin' class for dll '{dll}'", ex); }
            }
        }

        #endregion Methods
    }
}