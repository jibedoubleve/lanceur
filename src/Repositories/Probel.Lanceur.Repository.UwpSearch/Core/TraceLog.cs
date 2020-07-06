using Probel.Lanceur.Infrastructure;
using System;
using System.Runtime.CompilerServices;

namespace Probel.Lanceur.Repository.UwpSearch.Core
{
    internal class TraceLog : ILogService
    {
        #region Methods

        private void Write(string message, [CallerMemberName] string caller = null)
        {
            System.Diagnostics.Trace.WriteLine($"[{caller}] {message}");
        }

        public void Debug(string message) => Write(message);

        public void Debug(Exception ex) => Write(ex.ToString());

        public void Error(string message, Exception ex = null) => Write($"{message} [{ex}]");

        public void Fatal(string message, Exception ex = null) => Write($"{message} [{ex}]");

        public void Info(string message) => Write($"{message}");

        public void Trace(string message) => Write($"{message}");

        public void Warning(string message, Exception ex = null) => Write($"{message} [{ex}]");

        public void Warning(Exception ex) => Write($"{ex}");

        #endregion Methods
    }
}