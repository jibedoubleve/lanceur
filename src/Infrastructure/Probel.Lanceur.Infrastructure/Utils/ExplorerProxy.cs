﻿using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System.IO;

namespace Probel.Lanceur.Infrastructure.Utils
{
    public class ExplorerProxy
    {
        #region Fields

        private readonly ICommandRunner _cmdRunner;

        private readonly string _path;

        #endregion Fields

        #region Constructors

        public ExplorerProxy(string path, ICommandRunner cmdRunner)
        {
            _cmdRunner = cmdRunner;
            _path = path.Trim('"');
        }

        #endregion Constructors

        #region Methods

        public bool CanOpenInExplorer() => IsDirectory() || IsFile();

        public bool IsDirectory() => Directory.Exists(_path);

        public bool IsFile() => File.Exists(_path);

        public ExecutionResult OpenInExplorer() => _cmdRunner.Execute(Alias.FromPath(_path));

        #endregion Methods
    }
}