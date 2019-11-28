using Probel.Lanceur.Core.Services;
using System;
using System.Windows;
using Unity;

namespace Probel.Lanceur.Actions
{
    public class ClearAction : BaseUiAction
    {
        #region Fields

        private readonly IDataSourceService db;

        #endregion Fields

        #region Constructors

        public ClearAction(IUnityContainer container) : base(container)
        {
            db = container.Resolve<IDataSourceService>();
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg)
        {
            var nl = Environment.NewLine;
            var result = MessageBox.Show($"Do you want to erase all data from database?{nl}This action cannot be undone!", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes) { db.Clear(); }
        }

        #endregion Methods
    }
}