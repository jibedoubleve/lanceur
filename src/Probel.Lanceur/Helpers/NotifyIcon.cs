using Probel.Lanceur.Actions;
using Probel.Lanceur.Actions.Words;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WpfApplication = System.Windows.Application;

namespace Probel.Lanceur.Helpers
{
    public sealed class NotifyIconAdapter : IDisposable
    {
        #region Fields

        private readonly IActionContext _actionContext;
        private NotifyIcon _notifyIcon = new NotifyIcon();

        #endregion Fields

        #region Constructors

        public NotifyIconAdapter(IActionContext context, Action onShow)
        {
            var stream = WpfApplication.GetResourceStream(new Uri("pack://application:,,,/Probel.Lanceur;component/Assets/appIcon.ico")).Stream;
            _notifyIcon.Icon = new System.Drawing.Icon(stream);
            _notifyIcon.Visible = true;
            ContextMenu cm = BuildContextMenu();
            _actionContext = context;

            _notifyIcon.ContextMenu = cm;
            _notifyIcon.DoubleClick += OnDoubleClick;

            OnShow = onShow;
        }

        #endregion Constructors

        #region Properties

        private Action OnShow { get; }

        public bool Visible
        {
            get => _notifyIcon.Visible; set => _notifyIcon.Visible = value;
        }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }

        private ContextMenu BuildContextMenu()
        {
            var cm = new ContextMenu();
            var counter = 0;
            var l = new List<MenuItem>();

            l.Add(Menu(counter++, "Show...", OnShowImpl));
            l.Add(Menu(counter++, "Open settings...", OnShowSettingsImpl));
            l.Add(Menu(counter++, "Quit", OnQuitImpl));

            cm.MenuItems.AddRange(l.ToArray());
            return cm;
        }

        private MenuItem Menu(int counter, string text, Action<object, EventArgs> handler)
        {
            var m = new MenuItem
            {
                Index = counter,
                Text = text
            };
            m.Click += (sender, e) => handler(sender, e);
            return m;
        }

        private void OnDoubleClick(object sender, EventArgs e) => OnShowImpl(sender, e);

        private void OnQuitImpl(object sender, EventArgs e) => WpfApplication.Current.Shutdown();

        private void OnShowImpl(object sender, EventArgs e) => OnShow?.Invoke();

        private void OnShowSettingsImpl(object sender, EventArgs e)
        {
            var a = new SetupAction();
            a.With(_actionContext);
            a.Execute(string.Empty);
        }

        #endregion Methods
    }
}