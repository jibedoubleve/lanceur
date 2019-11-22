using Probel.Lanceur.Core.Constants;
using Probel.Lanceur.Core.Entities;
using System;
using System.Diagnostics;

namespace Probel.Lanceur.Core.Helpers
{
    public static class ConverterHelper
    {
        #region Methods

        public static ProcessWindowStyle AsWindowsStyle(this StartMode mode)
        {
            switch (mode)
            {
                case StartMode.Default: return ProcessWindowStyle.Normal;
                case StartMode.Maximised: return ProcessWindowStyle.Maximized;
                case StartMode.Minimised: return ProcessWindowStyle.Minimized;
                default:
                    throw new NotSupportedException($"The 'StartMode' of '{mode}' is not supported. Did you forget to support it?");
            }
        }

        #endregion Methods
    }
}