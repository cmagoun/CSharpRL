using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SadSharp.Helpers
{
    public static class Ticker
    {
        private static List<long> _ticks { get; set; }
        private static Stopwatch _ss { get; set; }
        private static string _msg { get; set; }
        private static bool _paused { get; set; }

        public static bool Paused => _paused;

        public static void Init()
        {
            _ticks = new List<long>();
        }

        public static void StartTicker(string msg = "")
        {
            _ss = Stopwatch.StartNew();
            _msg = msg;
        }

        public static void AddTick()
        {
            if (_ss != null) _ticks.Add(_ss.ElapsedTicks);
        }

        public static void MethodDone()
        {
            if (_paused) return;
            AddTick();
            Report();
        }

        public static void Pause()
        {
            _paused = true;
        }

        public static void Unpause()
        {
            _paused = false;
            _ticks.Clear();
        }

        public static void Report()
        {
            if (_paused) return;
            if (_ticks.Count > 0)
                Debug.WriteLine($"{(_msg != "" ? _msg : "Ticker")} Reports {_ticks.Count} calls of {(decimal)_ticks.Sum() / (decimal)_ticks.Count()} ticks");

            _ticks.Clear();
        }

        public static void MeasureHereToHere()
        {
            if (_paused) return;
            AddTick();
            Report();
            StartTicker();
        }
    }
}
