namespace Probel.Lanceur.Repositories
{
    public interface IAliasRepositoryBuilder
    {
        #region Properties

        bool IsInitialised { get; }

        #endregion Properties

        #region Methods

        IAliasRepository GetSource(string keyword);

        IAliasRepository GetSource(char? keyword);

        bool HasKeyword(string keyword);

        bool HasKeyword(char? keyword);

        void Initialise();

        string NormaliseQuery(string query);

        #endregion Methods
    }
}