using Newtonsoft.Json;
using Probel.Lanceur.Plugin.Clipboard.Models;
using System;
using System.IO;

namespace Probel.Lanceur.Plugin.Clipboard
{
    internal sealed class ClipboardManager : IDisposable
    {
        #region Fields

        private ClipboardCollection _history = null;

        #endregion Fields

        #region Properties

        private string FileName => Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\ClipboardHistory\history.json");

        #endregion Properties

        #region Methods

        public void AddClipboardContent()
        {
            var txt = System.Windows.Clipboard.GetText();
            GetHistory().Add(txt);
        }

        public void Dispose() => Save();

        public ClipboardCollection GetHistory()
        {
            if (_history == null)
            {
                CreateDefaultDir();

                if (File.Exists(FileName))
                {
                    var json = File.ReadAllText(FileName);
                    _history = JsonConvert.DeserializeObject<ClipboardCollection>(json);
                }
                else { _history = new ClipboardCollection(); }
            }
            return _history;
        }

        internal void Delete(ClipboardItem item) => GetHistory().Delete(item);

        private void CreateDefaultDir()
        {
            var dir = Path.GetDirectoryName(FileName);
            if (File.Exists(FileName) == false) { Directory.CreateDirectory(dir); }
        }

        private void Save()
        {
            CreateDefaultDir();
            var json = JsonConvert.SerializeObject(GetHistory());
            File.WriteAllText(FileName, json);
        }

        #endregion Methods
    }
}