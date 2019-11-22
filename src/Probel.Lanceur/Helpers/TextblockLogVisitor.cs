using Probel.Lanceur.Core.Services;
using System;
using System.Windows.Controls;

namespace Probel.Lanceur.Helpers
{
    public class TextblockLogVisitor : ILogVisitor
    {
        #region Fields

        private TextBlock _textBlock;

        #endregion Fields

        #region Methods

        public bool HasContext() => _textBlock != null;

        public void Intercept(string msg) => _textBlock.Text += $"{Environment.NewLine}{msg}";

        public void SetContext(object context)
        {
            if (context is TextBlock textBlock)
            {
                _textBlock = textBlock;
            }
        }

        #endregion Methods
    }
}