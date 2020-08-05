using Probel.Lanceur.Plugin.Clipboard.ViewModels;
using Probel.Lanceur.Plugin.Clipboard.Views;
using System.Linq;

namespace Probel.Lanceur.Plugin.Clipboard
{
    public class Plugin : PluginBase
    {
        #region Properties

        private ClipboardView View { get; set; }

        private ClipboardViewModel ViewModel => View.DataContext as ClipboardViewModel;

        #endregion Properties

        #region Methods

        public override void Execute(Cmdline cmd)
        {
            var list = new string[] { "list", "l" };

            var parameters = cmd.Arguments.ToLower().Trim();
            if (list.Contains(parameters))
            {
                ShowHistory();
            }
            else if (string.IsNullOrWhiteSpace(parameters))
            {
                using (var s = new ClipboardManager())
                {
                    s.AddClipboardContent();
                    ShowHistory();
                    Notifyer.NotifyInfo("Clipboard content saved.");
                }
            }
            else { Notifyer.NotifyWarning($"Not supported argument '{parameters}'. Use no argument to save clipboard or 'l' or 'list' to see history"); }
        }

        protected override void Initialise()
        {
            View = new ClipboardView();
            MainView.SetPluginArea(View);
        }

        private void ShowHistory()
        {
            ViewModel.Load();
            MainView.ShowPlugin();
        }

        #endregion Methods
    }
}