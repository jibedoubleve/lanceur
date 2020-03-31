using Humanizer;
using NCalc;
using Probel.Lanceur.Core.PluginsImpl;
using Probel.Lanceur.Plugin.Calculator.Models;
using Probel.Lanceur.Plugin.Calculator.ViewModels;
using Probel.Lanceur.Plugin.Calculator.Views;
using System.Data;

namespace Probel.Lanceur.Plugin.Calculator
{
    public class Plugin : PluginBase
    {
        #region Properties

        public ResultView View { get; private set; }
        public ResultViewModel ViewModel { get; private set; }

        #endregion Properties

        #region Methods

        public override void Execute(string parameters)
        {
            ViewModel.Log = Logger;
            ViewModel.Process(parameters);

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