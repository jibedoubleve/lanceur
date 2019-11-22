using Caliburn.Micro;
using System;

namespace Probel.Lanceur.Models.Settings
{
    public class WindowSettingsModel : PropertyChangedBase
    {
        #region Fields

        private byte _colourAlpha;
        private byte _colourBlue;
        private byte _coulorRed;
        private byte _coulourGreen;
        private double _opacity;
        private PositionSettingsModel _position;

        #endregion Fields

        #region Constructors

        public WindowSettingsModel()
        {
            Position = new PositionSettingsModel();
            Colour = "#FF1E1E1E";
            Opacity = 0.85;
        }

        #endregion Constructors

        #region Properties

        public string Colour
        {
            set => SetColour(value);
            get => $"#{ColourRed:X}{ColourGreen:X}{ColourBlue:X}{ColourAlpha:X}";
        }

        public byte ColourAlpha
        {
            get => _colourAlpha;
            set
            {
                if (Set(ref _colourAlpha, value, nameof(ColourAlpha)))
                {
                    NotifyOfPropertyChange(nameof(Colour));
                }
            }
        }

        public byte ColourBlue
        {
            get => _colourBlue;
            set
            {
                if (Set(ref _colourBlue, value, nameof(ColourBlue)))
                {
                    NotifyOfPropertyChange(nameof(Colour));
                }
            }
        }

        public byte ColourGreen
        {
            get => _coulourGreen;
            set
            {
                if (Set(ref _coulourGreen, value, nameof(ColourGreen)))
                {
                    NotifyOfPropertyChange(nameof(Colour));
                }
            }
        }

        public byte ColourRed
        {
            get => _coulorRed;
            set
            {
                if (Set(ref _coulorRed, value, nameof(ColourRed)))
                {
                    NotifyOfPropertyChange(nameof(Colour));
                }
            }
        }

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value, nameof(Opacity));
        }

        public PositionSettingsModel Position
        {
            get => _position;
            set => Set(ref _position, value, nameof(Position));
        }

        #endregion Properties

        #region Methods

        private void SetColour(string value)
        {
            if (value == null) { return; }
            else if (value.Length < 6) { return; }
            else if (value.Length > 0 && value[0] == '#') { value = value.Substring(1, value.Length - 1); }

            var rs = value.Substring(0, 2);
            ColourRed = Convert.ToByte(rs, 16);

            var gs = value.Substring(2, 2);
            ColourGreen = Convert.ToByte(gs, 16);

            var bs = value.Substring(4, 2);
            ColourBlue = Convert.ToByte(bs, 16);

            if (value.Length == 8)
            {
                var al = value.Substring(6, 2);
                ColourAlpha = Convert.ToByte(al, 16);
            }

            NotifyOfPropertyChange(nameof(Colour));
        }

        #endregion Methods
    }
}