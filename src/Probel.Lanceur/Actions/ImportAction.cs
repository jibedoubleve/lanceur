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

        #region Properties

        private ISlickRunImporterService Importer
        {
            get
            {
                if (_importer == null) { _importer = Container.Resolve<ISlickRunImporterService>(); }
                return _importer;
            }
        }

        #endregion Properties

        #region Methods

        public override void Execute(string arg) => Importer.Import();

        #endregion Methods
    }
}