﻿namespace Probel.Lanceur.Plugin.Spotify.Helpers
{
    public static class ParametersExtensions
    {
        #region Fields

        private static readonly string[] NextPattern = new string[] { "n", "next", ">", ">>", "nxt" };
        private static readonly string[] PreviousPattern = new string[] { "p", "previous", "prev", "<", "<<" };
        private static readonly string[] RestartSong = new string[] { "r", "restart" };

        #endregion Fields

        #region Methods

        private static bool Is(this PluginCmdline cmd, string[] patterns)
        {
            var arg = cmd.Arguments.ToLower();
            foreach (var pattern in patterns)
            {
                if (pattern.ToLower() == arg) { return true; }
            }
            return false;
        }

        public static bool IsNextSong(this PluginCmdline cmd) => Is(cmd, NextPattern);

        public static bool IsPreviousSong(this PluginCmdline cmd) => Is(cmd, PreviousPattern);

        public static bool IsRestartSong(this PluginCmdline cmd) => Is(cmd, RestartSong);

        #endregion Methods
    }
}