namespace Probel.Lanceur.Core.Plugins
{
    internal class EmptyPlugin : PluginBase
    {
        #region Methods

        public override void Execute(string parameters) => Logger.Warning("User tried to execute an empty plugin.");

        #endregion Methods
    }
}