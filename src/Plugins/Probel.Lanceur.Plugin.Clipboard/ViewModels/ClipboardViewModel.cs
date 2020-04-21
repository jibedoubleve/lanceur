using Caliburn.Micro;
using Probel.Lanceur.Plugin.Clipboard.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.Lanceur.Plugin.Clipboard.ViewModels
{
    internal class ClipboardViewModel : PropertyChangedBase
    {
        #region Fields

        private ObservableCollection<ClipboardItem> _history;

        #endregion Fields

        #region Properties

        public ObservableCollection<ClipboardItem> History
        {
            get => _history;
            set => Set(ref _history, value, nameof(History));
        }

        #endregion Properties

        #region Methods

        public void Delete(ClipboardItem item)
        {
            using (var cm = new ClipboardManager())
            {
                cm.Delete(item);
            }
        }

        public void Load()
        {
            using (var cm = new ClipboardManager())
            {
                var h = cm.GetHistory();
                History = new ObservableCollection<ClipboardItem>(h.History.OrderByDescending(e => e.Saved));
            }
        }

        #endregion Methods
    }
}