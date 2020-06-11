using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Media;

namespace HistoryViewer.Infrastructure
{
    public class ImageCache
    {
        #region Fields

        private static ImageCache _instance = null;
        private readonly ConcurrentDictionary<string, ImageSource> _cache = new ConcurrentDictionary<string, ImageSource>();

        #endregion Fields

        #region Constructors

        private ImageCache()
        {
        }

        private ImageCache(IEnumerable<string> keys)
        {
        }

        #endregion Constructors

        #region Properties

        private static string ImageCachePath => Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["ImageCache"]);

        public static ImageCache Instance
        {
            get
            {
                if (_instance != null) { return _instance; }
                else if (File.Exists(ImageCachePath))
                {
                    _instance = new ImageCache(GetKeys());
                    return _instance;
                }
                else
                {
                    _instance = new ImageCache();
                    return _instance;
                }
            }
        }

        public IEnumerable<string> Keys => _cache?.Keys ?? new string[] { };

        #endregion Properties

        #region Indexers

        public ImageSource this[string path]
        {
            get => _cache[path];
            set => _cache[path] = value;
        }

        #endregion Indexers

        #region Methods

        public static IEnumerable<string> GetKeys()
        {
            var json = File.ReadAllText(ImageCachePath);
            var keys = JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            return keys;
        }

        public static void Save()
        {
            if (_instance != null)
            {
                var json = JsonConvert.SerializeObject(_instance._cache.Keys, Formatting.Indented);
                File.WriteAllText(ImageCachePath, json);
            }
        }

        public bool ContainsKey(string key)
        {
            var contains = _cache.ContainsKey(key);
            return contains;
        }

        #endregion Methods
    }
}