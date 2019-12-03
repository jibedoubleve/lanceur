using Probel.Lanceur.Core.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class ImportAction : BaseUiAction
    {
        #region Fields

        private ISlickRunImporterService _importer;

        #endregion Fields

        #region Constructors

        protected override void Configure()
        {
            _importer = Container.Resolve<ISlickRunImporterService>();
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg) => _importer.Import();

        #endregion Methods
    }
}