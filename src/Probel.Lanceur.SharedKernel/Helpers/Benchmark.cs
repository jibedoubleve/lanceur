using System;
using System.Diagnostics;

namespace Probel.Lanceur.SharedKernel.Helpers
{
    public class Benchmark : IDisposable
    {
        #region Fields

        private readonly Action<TimeSpan> _onStop;
        private readonly Stopwatch _stopwatch;
        private readonly string _tag;

        #endregion Fields

        #region Constructors

        public Benchmark(string tag = null, Action<TimeSpan> onStop = null)
        {
            _onStop = onStop;
            _tag = tag;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public Benchmark(Action<TimeSpan> onStop) : this(null, onStop)
        {
        }

        #endregion Constructors

        #region Methods

        public void Dispose() => Flush();

        public void Flush()
        {
            _stopwatch.Stop();
            Debug.WriteLine($"{_tag,-20} - Elapsed time: {(float)_stopwatch.ElapsedMilliseconds / 1000} sec.");
            _onStop?.Invoke(_stopwatch.Elapsed);
            _stopwatch.Start();
        }

        #endregion Methods
    }
}