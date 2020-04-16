using Probel.Lanceur.Core.Services;
using System;
using OutputTrace = System.Diagnostics.Trace;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class TraceLogger : ILogService
    {
        #region Fields

        private const string TEMPLATE = "[{0,-7}] {1}";

        #endregion Fields

        #region Enums

        private enum Level
        {
            Fatal,
            Error,
            Warning,
            Info,
            Debug,
            Trace,
        }

        #endregion Enums

        #region Methods

        public void Debug(string message) => WriteLine(Level.Debug, message);

        public void Debug(Exception ex) => Debug(ex.ToString());

        public void Error(string message, Exception ex = null) => WriteLine(Level.Warning, message, ex);

        public void Fatal(string message, Exception ex = null) => WriteLine(Level.Warning, message, ex);

        public void Info(string message) => WriteLine(Level.Info, message);

        public void Trace(string message) => WriteLine(Level.Trace, message);

        public void Warning(string message, Exception ex = null) => WriteLine(Level.Warning, message, ex);

        public void Warning(Exception ex) => WriteLine(Level.Warning, ex.Message, ex);

        private void WriteLine(Level level, string message, Exception ex = null)
        {
            var msg = string.Format(TEMPLATE, level.ToString().ToUpper(), message);
            if (ex != null)
            {
                msg += Environment.NewLine + ex.ToString();
            }
            OutputTrace.WriteLine(msg);
        }

        #endregion Methods
    }
}