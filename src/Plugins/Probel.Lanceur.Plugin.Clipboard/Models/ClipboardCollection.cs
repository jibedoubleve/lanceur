using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Plugin.Clipboard.Models
{
    internal class ClipboardCollection
    {
        #region Fields

        private readonly List<ClipboardItem> _history = new List<ClipboardItem>();

        #endregion Fields

        #region Properties

        public IEnumerable<ClipboardItem> History => _history;

        #endregion Properties

        #region Methods

        public void Add(ClipboardItem item)
        {
            _history.Add(item);
        }

        public void Add(string txt)
        {
            Add(new ClipboardItem
            {
                Text = txt,
                Saved = DateTime.Now
            });
        }

        public void Delete(ClipboardItem item)
        {
            var del = (from i in _history
                       where i.Saved == item.Saved
                       && i.Text == item.Text
                       select i).FirstOrDefault();

            if (del != null) { _history.Remove(del); }
        }

        #endregion Methods
    }
}