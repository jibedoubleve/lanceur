using Caliburn.Micro;
using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Models
{
    public class ResultItem : PropertyChangedBase
    {
        #region Fields

        private string _footer;
        private string _icon;
        private string _subTitle;

        private string _title;

        #endregion Fields

        #region Properties

        public virtual string CmdLine { get; }

        public string Footer
        {
            get => _footer;
            set => Set(ref _footer, value, nameof(Footer));
        }

        public string Icon
        {
            get => _icon;
            set => Set(ref _icon, value, nameof(Icon));
        }

        public string Subtitle
        {
            get => _subTitle;
            set => Set(ref _subTitle, value, nameof(Subtitle));
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value, nameof(Title));
        }

        #endregion Properties
    }

    public class SwitchSessionResult : ResultItem
    {
        #region Properties

        public override string CmdLine => $"switch {Title}";

        #endregion Properties

        #region Methods

        public static explicit operator AliasText(SwitchSessionResult src)
        {
            return new AliasText
            {
                Name = src.CmdLine,
            };
        }

        #endregion Methods
    }
}