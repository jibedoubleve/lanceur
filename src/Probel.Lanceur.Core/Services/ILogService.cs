using System;

namespace Probel.Lanceur.Core.Services
{
    public interface ILogService
    {
        #region Methods

        void Debug(string message);

        void Debug(Exception ex);

        void Fatal(string message, Exception ex = null);

        void Trace(string message);

        void Warning(string message, Exception ex = null);

        #endregion Methods
    }
}