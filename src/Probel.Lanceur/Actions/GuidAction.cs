﻿using System;
using System.Windows;

namespace Probel.Lanceur.Actions
{
    [UiAction]
    public class GuidAction : BaseUiAction
    {
        #region Methods

        protected override void Configure()
        {
        }

        protected override void DoExecute(string arg)
        {
            var guid = Guid.NewGuid();
            Clipboard.SetText(guid.ToString());
        }

        #endregion Methods
    }
}