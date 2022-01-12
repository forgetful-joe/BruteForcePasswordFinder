using System.Diagnostics;
using Humanizer;

namespace BruteForcePasswordFinder.Helpers
{
    public class TimedAction : IDisposable
    {
        private readonly string action;
        private readonly ushort level;
        Stopwatch stopwatch;
        public TimedAction(string action, ushort level = 0)
        {
            stopwatch = new Stopwatch();
            this.action = action;
            this.level = level;
            Helper.Write($"Started [{action}]", level);
            stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            Helper.Write($"Ended [{action}], took {stopwatch.Elapsed.Humanize(precision: 2)}", level);
        }
    }
}
