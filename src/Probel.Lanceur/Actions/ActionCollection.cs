using Probel.Lanceur.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Actions
{
    public sealed class ActionCollection : IActionCollection, IEnumerable<ActionWord>, IEnumerator<ActionWord>
    {
        #region Fields

        private IList<ActionWord> _actions;

        private int _index = -1;
        private IEnumerable<Type> _types;

        #endregion Fields

        #region Properties

        public ActionWord Current => Actions[_index];

        object IEnumerator.Current => Current;

        public IEnumerable<string> Keywords => from a in Actions
                                               select a.Name.ToLower();

        private IList<ActionWord> Actions
        {
            get
            {
                if (_actions == null)
                {
                    var tmp = new List<ActionWord>();
                    foreach (var type in GetTypes())
                    {
                        var n = GetActionName(type).ToUpper();
                        var d = GetDescription(type);
                        var t = type;
                        tmp.Add(new ActionWord(t, n, d));
                    }
                    _actions = tmp;
                }
                return _actions;
            }
        }

        #endregion Properties

        #region Indexers

        public ActionWord this[int i] => Actions[i];

        public ActionWord this[Type t]
        {
            get
            {
                var r = (from a in Actions
                         where a.Type == t
                         select a).FirstOrDefault();
                return r;
            }
        }

        #endregion Indexers

        #region Methods

        public void Dispose() => _index = 0;

        public IEnumerator<ActionWord> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            ++_index;
            return (_index < Actions.Count);
        }

        public void Reset() => _index = -1;

        public IEnumerable<ActionWord> ToList() => Actions;

        private string GetActionName(Type type)
        {
            var ca = type.GetCustomAttribute<UiActionAttribute>();
            if (string.IsNullOrEmpty(ca?.Action))
            {
                var name = type.Name.Replace("Action", "");
                return name;
            }
            else { return ca?.Action ?? "N.A."; }
        }

        private string GetDescription(Type type)
        {
            var ca = type.GetCustomAttribute<DescriptionAttribute>();
            if (string.IsNullOrEmpty(ca?.Description))
            {
                return string.Empty;
            }
            else { return ca?.Description ?? string.Empty; }
        }

        private IEnumerable<Type> GetTypes()
        {
            if (_types == null)
            {
                _types = from t in Assembly.GetAssembly(typeof(IUiAction)).GetTypes()
                         where t.GetCustomAttribute<UiActionAttribute>() != null
                         select t;
            }
            return _types;
        }

        #endregion Methods
    }
}