using Caliburn.Micro;
using Probel.Lanceur.Core.Constants;

namespace Probel.Lanceur.Models
{
    public class BaseShortcutModel : PropertyChangedBase
    {
        #region Fields

        private string _arguments;
        private string _fileName;
        private long _id;
        private bool _isExecutable;
        private string _notes;
        private RunAs _runAs;
        private StartMode _startMode;

        #endregion Fields

        #region Properties

        public string Arguments
        {
            get => _arguments;
            set => Set(ref _arguments, value, nameof(Arguments));
        }

        public string FileName
        {
            get => _fileName;
            set => Set(ref _fileName, value, nameof(FileName));
        }

        public long Id
        {
            get => _id;
            set => Set(ref _id, value, nameof(Id));
        }

        public bool IsExecutable
        {
            get => _isExecutable;
            set => Set(ref _isExecutable, value, nameof(IsExecutable));
        }

        public string Notes
        {
            get => _notes;
            set => Set(ref _notes, value, nameof(Notes));
        }

        public RunAs RunAs
        {
            get => _runAs;
            set => Set(ref _runAs, value, nameof(RunAs));
        }

        public StartMode StartMode
        {
            get => _startMode;
            set => Set(ref _startMode, value, nameof(StartMode));
        }

        #endregion Properties
    }
}