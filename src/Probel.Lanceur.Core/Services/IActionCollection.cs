using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IActionCollection
    {
        #region Properties

        ActionWord Current { get; }

        IEnumerable<string> Keywords { get; }

        #endregion Properties

        #region Indexers

        ActionWord this[int i] { get; }
        ActionWord this[Type t] { get; }

        #endregion Indexers

        #region Methods

        void Dispose();

        IEnumerator<ActionWord> GetEnumerator();

        bool MoveNext();

        void Reset();

        IEnumerable<ActionWord> ToList();

        #endregion Methods
    }

    public class ActionWord
    {
        #region Constructors

        public ActionWord(Type type, string name, string description)
        {
            Type = type;
            Name = name.ToUpper();
            Description = description;
        }

        #endregion Constructors

        #region Properties

        public string Description { get; private set; }
        public string Name { get; private set; }
        public Type Type { get; private set; }

        #endregion Properties
    }
}