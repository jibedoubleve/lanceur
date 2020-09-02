namespace Probel.Lanceur.Repositories
{
    public interface IRepositorySettings
    {
        #region Properties

        /// <summary>
        /// Gets the threshold value where the result is to be selected or not
        /// </summary>
        float ScoreLimit { get; }

        #endregion Properties
    }
}