using Unity;

namespace Probel.Lanceur.Actions
{
    public interface IUiAction
    {
        #region Methods

        void Execute(string arg);

        BaseUiAction With(IUnityContainer container);

        #endregion Methods
    }
}