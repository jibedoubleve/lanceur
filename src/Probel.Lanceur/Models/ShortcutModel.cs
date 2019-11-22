namespace Probel.Lanceur.Models
{
    public class ShortcutModel : BaseShortcutModel
    {
        #region Fields

        private string _name;

        #endregion Fields

        #region Properties

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        #endregion Properties
    }
}