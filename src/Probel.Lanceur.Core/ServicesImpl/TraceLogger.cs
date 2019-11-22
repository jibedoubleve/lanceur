using Probel.Lanceur.Core.Services;
using System;
using System.Diagnostics;

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

        private void WriteLine(Level level, string message, Exception ex = null)
        {
            var msg = string.Format(TEMPLATE, level.ToString().ToUpper(), message);
            if (ex != null)
            {
                msg += Environment.NewLine + ex.ToString();
            }
            Trace.WriteLine(msg);
        }

        public void Debug(string message) => WriteLine(Level.Debug, message);

        public void Debug(Exception ex) => Debug(ex.ToString());

        public void Warning(string message, Exception ex) => WriteLine(Level.Warning, message, ex);

        #endregion Methods
    }
}