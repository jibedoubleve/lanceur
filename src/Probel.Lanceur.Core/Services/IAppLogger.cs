using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probel.Lanceur.Core.Services
{
    public interface IAppLogger
    {
        #region Methods

        void Debug(string msg);

        void Error(string msg);

        void Error(Exception ex);

        void Fatal(string msg);

        void Trace(string msg);

        void Warn(string msg);

        #endregion Methods
    }
}
