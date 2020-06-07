using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.Plugin.Evernote.Services
{
    internal class Configurator
    {
        #region Fields

        private readonly string _parameters;
        private Dictionary<string, string> _configuration = null;
        private Regex _regex = new Regex(@"^(?<key>(key|host|server)):(?<value>.*)");

        #endregion Fields

        #region Constructors

        public Configurator(string parameters)
        {
            this._parameters = parameters;
        }

        #endregion Constructors

        #region Properties

        public string Host => Get("host");

        public string Key => Get("key");

        public string Server => Get("server");

        #endregion Properties

        #region Methods

        public bool IsValid() => _parameters.StartsWith("-c") || _parameters.StartsWith("config");

        private string Get(string key)
        {
            LoadConfiguration();
            return _configuration.ContainsKey(key)
                ? _configuration[key]
                : string.Empty;
        }

        private string[] GetSplits() => _parameters.Split(' ');

        private void LoadConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new Dictionary<string, string>();
                var splits = GetSplits();

                foreach (var split in splits)
                {
                    var groups = _regex.Match(split)?.Groups;
                    var key = groups["key"].Value;
                    var value = groups["value"].Value;

                    if (_configuration.ContainsKey(key)) { throw new ArgumentException($"Evernote configuration is not valid. Configuration key '{key}' is already set."); }
                    else { _configuration.Add(key, value); }
                }
            }
        }

        #endregion Methods
    }
}