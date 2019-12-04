using System;

namespace Probel.Lanceur.Core.Services
{
    public interface ISlickRunImporterService
    {
        #region Events

        event EventHandler<ImportUpdatedEventArg> ImportUpdated;

        #endregion Events

        #region Methods

        long Import(string sessionName = null, string fileName = null);

        #endregion Methods
    }

    public class ImportUpdatedEventArg : EventArgs
    {
        #region Constructors

        public ImportUpdatedEventArg(int progress, string output)
        {
            Output = output;
            Progress = progress;
        }

        #endregion Constructors

        #region Properties

        public string Output { get; }
        public int Progress { get; }

        #endregion Properties
    }
}