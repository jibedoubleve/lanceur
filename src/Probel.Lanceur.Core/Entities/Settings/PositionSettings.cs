using System.Diagnostics;

namespace Probel.Lanceur.Core.Entities.Settings
{
    [DebuggerDisplay("Left:{Left} - Top:{Top}")]
    public class PositionSettings
    {
        #region Properties

        public double Left { get; set; } = 0;
        public double Top { get; set; } = 0;

        #endregion Properties
    }
}