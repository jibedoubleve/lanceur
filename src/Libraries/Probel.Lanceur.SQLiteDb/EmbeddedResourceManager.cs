﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.SQLiteDb
{
    internal class EmbeddedResourceManager
    {
        #region Fields

        private readonly Assembly ExecutingAssembly;

        #endregion Fields

        #region Constructors

        public EmbeddedResourceManager(Assembly executingAssembly) => ExecutingAssembly = executingAssembly;

        public EmbeddedResourceManager() => ExecutingAssembly = Assembly.GetExecutingAssembly();

        #endregion Constructors

        #region Methods

        public void CopyTo(string resource, string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            dir = Environment.ExpandEnvironmentVariables(dir);

            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            using (var stream = GetResource(resource))
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                if (stream == null) { throw new ArgumentException($"The resource stream was 'NULL'. Did you misspelled the resource? [name: {resource}]"); }
                stream.CopyTo(fileStream);
            }
        }

        public Stream GetResource(string resourceName) => ExecutingAssembly.GetManifestResourceStream(resourceName);

        public void ReadResourceAsString(string resourceName, Action<string> OnResource)
        {
            using (var stream = GetResource(resourceName))
            {
                if (stream == null) { throw new ArgumentNullException($"'{nameof(resourceName)}' is null. Are you sure the resource '{resourceName}' exists in the assembly '{ExecutingAssembly.FullName}'"); }
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    OnResource(text);
                }
            }
        }

        public IEnumerable<string> ListResources(string pattern)
        {
            var result = (from s in ExecutingAssembly.GetManifestResourceNames()
                          where s.Contains(pattern)
                          select s);
            return result;
        }

        public IEnumerable<string> ListResources(Regex regex)
        {
            var result = (from s in ExecutingAssembly.GetManifestResourceNames()
                          where regex.IsMatch(s)
                          select s);
            return result;
        }

        #endregion Methods
    }
}