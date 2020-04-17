using System;

namespace Probel.Lanceur.Plugin
{
    public interface ILogService
    {
        #region Methods

        void Debug(string message);

        void Debug(Exception ex);

        void Error(string message, Exception ex = null);

        void Fatal(string message, Exception ex = null);

        void Info(string message);

        void Trace(string message);

        void Warning(string message, Exception ex = null);

        void Warning(Exception ex);

        #endregion Methods
    }
}