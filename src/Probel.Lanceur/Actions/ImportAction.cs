using Probel.Lanceur.Core.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    public class ImportAction : BaseUiAction
    {
        #region Fields

        private ISlickRunImporterService importer;

        #endregion Fields

        #region Constructors

        public ImportAction(IUnityContainer container) : base(container)
        {
            importer = Container.Resolve<ISlickRunImporterService>();
        }

        #endregion Constructors

        #region Methods

        public override void Execute(string arg) => importer.Import();

        #endregion Methods
    }
}