using Unity;

namespace Probel.Lanceur.Actions
{
    public interface IUiAction
    {
        #region Methods

        void Execute(string arg);

        IUiAction With(IUnityContainer container);

        #endregion Methods
    }
}