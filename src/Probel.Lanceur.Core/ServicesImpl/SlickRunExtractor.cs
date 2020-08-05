using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using Probel.Lanceur.Core.Constants;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class SlickRunExtractor : ISlickRunExtractor
    {
        #region Fields

        private string DefaultFileName => @"%AppData%\SlickRun\SlickRun.srl";

        #endregion Fields

        #region Constructors

        public SlickRunExtractor()
        {
        }

        #endregion Constructors

        #region Methods

        private static Dictionary<string, string> GetDictionary(string[] lines)
        {
            var length = lines.Length - 1;
            var info = new string[length];
            Array.Copy(lines, 1, info, 0, length);

            var dico = new Dictionary<string, string>();
            foreach (var i in info)
            {
                var key = i.Split('=');
                var values = new string[key.Length - 1];
                Array.Copy(key, 1, values, 0, key.Length - 1);
                var value = values.Aggregate((partial, current) => $"{partial}={current}");

                dico.Add(key[0].ToLower(), value);
            }
            return dico;
        }

        private static RunAs GetRunAs(Dictionary<string, string> dico)
        {
            if (dico.ContainsKey("userunas"))
            {
                if (int.TryParse(dico["userunas"], out var integer))
                {
                    return (integer == -1) ? RunAs.Admin : RunAs.CurrentUser;
                }
                else { return RunAs.CurrentUser; }
            }
            else { return RunAs.CurrentUser; }
        }

        private MultiNameAlias BuildEntry(string item)
        {
            var lines = item.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var name = lines[0].Replace("]", "");

            var dico = GetDictionary(lines);

            return new MultiNameAlias()
            {
                Names = name.Split(','),
                FileName = dico.ContainsKey("filename") ? dico["filename"] : string.Empty,
                Arguments = dico.ContainsKey("params") ? dico["params"] : string.Empty,
                Notes = dico.ContainsKey("notes") ? dico["notes"] : string.Empty,
                RunAs = GetRunAs(dico),
                WorkingDirectory = dico.ContainsKey("path") ? dico["path"] : string.Empty,
            };
        }

        private IEnumerable<string> SplitedEntries(string txt)
        {
            var separator = new char[] { '[' };
            return txt.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerable<MultiNameAlias> Extract(string fileName = null)
        {
            fileName = Environment.ExpandEnvironmentVariables(fileName ?? DefaultFileName);
            var txt = File.ReadAllText(fileName);
            var result = new List<MultiNameAlias>();

            foreach (var item in SplitedEntries(txt))
            {
                result.Add(BuildEntry(item));
            }

            return result;
        }

        #endregion Methods
    }
}