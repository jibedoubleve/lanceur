using Probel.Lanceur.Core.Services;
using System;
using System.Windows;
using Unity;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
    public class ClearAction : BaseUiAction
    {
        #region Fields

        private IDataSourceService _db;

        #endregion Fields

        #region Methods

        protected override void Configure()
        {
            _db = Container.Resolve<IDataSourceService>();
        }

        protected override void DoExecute(string arg)
        {
            var nl = Environment.NewLine;
            var result = MessageBox.Show($"Do you want to erase all data from database?{nl}This action cannot be undone!", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes) { _db.Clear(); }
        }

        #endregion Methods
    }
}