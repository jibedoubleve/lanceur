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

        public override void Execute(PluginCmdline cmd)
        {
            ViewModel.Log = Logger;

            var exp = string.IsNullOrEmpty(cmd?.Arguments) ? cmd?.Command ?? string.Empty : cmd.Arguments;

            ViewModel.Process(exp);

            MainView.ShowPlugin();
        }

        protected override void Initialise()
        {
            View = new ResultView();
            ViewModel = View.DataContext as ResultViewModel;

            MainView.SetPluginArea(View);
        }

        #endregion Methods
    }
}