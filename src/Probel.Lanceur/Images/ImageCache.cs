using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Media;

namespace Probel.Lanceur.Images
{
    public class ImageCache
    {
        #region Fields

        private readonly ConcurrentDictionary<string, ImageSource> _cache = new ConcurrentDictionary<string, ImageSource>();
        public readonly ConcurrentDictionary<string, int> Usage;

        #endregion Fields

        #region Constructors

        public ImageCache()
        {
            Usage = new ConcurrentDictionary<string, int>();
        }

        public ImageCache(IDictionary<string, int> src)
        {
            Usage = new ConcurrentDictionary<string, int>(src);
        }

        #endregion Constructors

        #region Properties

        private static string ImageCachePath => Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["ImageCache"]);

        #endregion Properties

        #region Indexers

        public ImageSource this[string path]
        {
            get
            {
                Usage.AddOrUpdate(path, 1, (k, v) => v + 1);
                var i = _cache[path];
                return i;
            }
            set
            {
                if (value != null)
                {
                    _cache[path] = value;
                }
            }
        }

        #endregion Indexers

        #region Methods

        public bool ContainsKey(string key)
        {
            var contains = _cache.ContainsKey(key);
            return contains;
        }

        public IEnumerable<string> GetKeys() => File.Exists(ImageCachePath) ? Usage.Keys : (new string[] { });

        #endregion Methods
    }
}