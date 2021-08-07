using System;

namespace Probel.Lanceur.Models
{
    public class AliasModel : BaseAliasModel
    {
        #region Fields

        private string _icon;
        private long _idSession;
        private bool _isPackage;
        private string _name;

        private string _uniqueIdentifyer;
        private string _workingDirectory;

        #endregion Fields

        #region Properties

        public string Icon
        {
            get => _icon;
            set => Set(ref _icon, value);
        }

        public long IdSession
        {
            get => _idSession;
            set => Set(ref _idSession, value);
        }

        public bool IsPackage
        {
            get => _isPackage;
            set => Set(ref _isPackage, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        public string UniqueIdentifyer
        {
            get => _uniqueIdentifyer;
            set => Set(ref _uniqueIdentifyer, value);
        }

        public string WorkingDirectory
        {
            get => _workingDirectory;
            set => Set(ref _workingDirectory, value);
        }

        #endregion Properties
    }
}