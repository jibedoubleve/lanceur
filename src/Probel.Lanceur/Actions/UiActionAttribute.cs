using Probel.Lanceur.Core.Constants;
using System;

namespace Probel.Lanceur.Actions
{
    /// <summary>
    /// UiAction is the action bound on a reserved keyword. These keywords are
    /// defined in the Enum <see cref="Keywords"/>. 
    ///   * Use the default constructor when the name of the class (child of 
    ///     BaseUiAction and that implements IUiAction) is build on this pattern 
    ///     "xxxAction" where "xxx" is the name (no matter the case) of one of
    ///     the element of <see cref="Keywords"/>.
    ///   * Use the other constructor when the class does not follow the pattern
    ///     described above. The argument 'action' shoud be the name of one of
    ///     the element of <see cref="Keywords"/>.
    /// </summary>
    public sealed class UiActionAttribute : Attribute
    {
        #region Constructors

        public UiActionAttribute(string action)
        {
            Action = action;
        }

        public UiActionAttribute() : this(string.Empty)
        {
        }

        #endregion Constructors

        #region Properties

        public string Action { get; }

        #endregion Properties
    }
}