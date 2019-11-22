namespace Probel.Lanceur.Core.Services
{
    public interface ILogVisitor
    {
        #region Methods

        void Intercept(string msg);

        void SetContext(object context);
        bool HasContext();

        #endregion Methods
    }
}