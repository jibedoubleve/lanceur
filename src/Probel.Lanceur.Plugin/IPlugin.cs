namespace Probel.Lanceur.Plugin
{
    public interface IPlugin
    {
        #region Methods

        void Execute(Cmdline parameters);

        void Initialise(IPluginContext context);

        #endregion Methods
    }
}