using Probel.Lanceur.Plugin.Clipboard.ViewModels;
using Probel.Lanceur.Plugin.Clipboard.Views;

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
            var parameters = cmd.Parameters.ToLower().Trim();
            if (!parameters.StartsWith("list"))
            {
                using (var s = new ClipboardManager())
                {
                    s.AddClipboardContent();
                    Notifyer.NotifyInfo("Clipboard content saved.");
                }
            }

            ShowHistory();
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