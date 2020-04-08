namespace Probel.Lanceur.Models
{
    public class NewAliasSessionModel : AliasSessionModel
    {
        #region Fields

        private const string DefaultMessage = "New session";

        #endregion Fields

        #region Constructors

        public NewAliasSessionModel()
        {
            Name = DefaultMessage;
        }

        #endregion Constructors

        #region Properties

        public string Message => "<Create new session>";

        #endregion Properties

        #region Methods

        public void Reset() => Name = DefaultMessage;

        #endregion Methods
    }
}