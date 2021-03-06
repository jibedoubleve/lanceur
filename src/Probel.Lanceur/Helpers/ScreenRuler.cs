﻿using Probel.Lanceur.Core.Helpers;
using System.Windows;

namespace Probel.Lanceur.Helpers
{
    public class ScreenRuler : IScreenRuler
    {
        #region Properties

        public double MaxLeft => SystemParameters.WorkArea.Width;
        public double MaxTop => SystemParameters.WorkArea.Height;

        #endregion Properties

        #region Methods

        public double GetWindowHeight() => Application.Current.MainWindow.Height;

        public double GetWindowWidth() => Application.Current.MainWindow.Width;

        public Coordinate StickTo(double left, double top)
        {
            var topScale = MaxTop / 2;
            var leftScale = MaxLeft / 2;

            var wHeight = GetWindowHeight();
            var wWidth = GetWindowWidth();

            var rLeft = (left <= leftScale) ? 0 : (MaxLeft - wWidth);
            var rTop = (top <= topScale) ? 0 : (MaxTop - wHeight);

            return new Coordinate(rLeft, rTop);
        }

        public Coordinate Center(double distanceFromTop)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            var windowWidth = GetWindowWidth();
            //var windowHeight = height ?? GetWindowHeight();

            var rLeft = (screenWidth / 2) - (windowWidth / 2);
            //var rTop = (screenHeight / 2) - (windowHeight / 2);
            var rTop = distanceFromTop;

            return new Coordinate(rLeft, rTop);
        }

        #endregion Methods
    }
}