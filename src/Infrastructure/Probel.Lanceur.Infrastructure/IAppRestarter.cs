using System.Threading.Tasks;

namespace Probel.Lanceur.Infrastructure
{
    public interface IAppRestarter
    {
        #region Methods

        Task<bool> DoRestartAsync();

        void Restart();

        #endregion Methods
    }
}