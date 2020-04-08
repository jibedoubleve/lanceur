using System.Threading;

namespace Probel.Lanceur.Helpers
{
    public static class SingleInstance
    {
        #region Fields

        private static Mutex _mutex = new Mutex(true, @"Global\Lanceur");

        #endregion Fields

        #region Methods

        public static void ReleaseMutex() => _mutex.ReleaseMutex();

        public static bool WaitOne()
        {
            try { return _mutex.WaitOne(100); }
            catch (AbandonedMutexException) { return false; }
        }

        #endregion Methods
    }
}