using System;
using System.Diagnostics;

namespace Probel.Lanceur.SharedKernel.Helpers
{
    public class Benchmark : IDisposable
    {
        #region Fields

        private readonly Stopwatch _stopwatch;
        private readonly string _tag;

        #endregion Fields

        #region Constructors

        public Benchmark(string tag = null)
        {
            _tag = tag;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        #endregion Constructors

        #region Methods

        public void Dispose() => Flush();

        public void Flush()
        {
            _stopwatch.Stop();
            Debug.WriteLine($"{_tag,-20} - Elapsed time: {(float)_stopwatch.ElapsedMilliseconds / 1000} sec.");
            _stopwatch.Start();
        }

        #endregion Methods
    }
}