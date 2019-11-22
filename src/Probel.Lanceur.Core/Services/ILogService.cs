using System;

namespace Probel.Lanceur.Core.Services
{
    public interface ILogService
    {
        #region Methods

        void Debug(string message);

        void Debug(Exception ex);

        void Warning(string message, Exception ex = null);

        #endregion Methods
    }
}