using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.PluginsImpl;
using Probel.Lanceur.Plugin.Calculator.ViewModels;
using Probel.Lanceur.Plugin.Calculator.Views;

namespace Probel.Lanceur.Plugin.Calculator
{
    public class Plugin : PluginBase
    {
        #region Properties

        public ResultView View { get; private set; }

        public ResultViewModel ViewModel { get; private set; }

        #endregion Properties

        #region Methods

        public override void Execute(Cmdline cmd)
        {
            ViewModel.Log = Logger;

            var exp = string.IsNullOrEmpty(cmd?.Parameters) ? cmd?.Command ?? string.Empty : cmd.Parameters;

            ViewModel.Process(exp);

            MainView.HideResults();
        }

        protected override void Boot()
        {
            View = new ResultView();
            ViewModel = View.DataContext as ResultViewModel;

            MainView.SetPluginArea(View);
        }

        #endregion Methods
    }
}