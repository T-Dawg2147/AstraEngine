namespace AstraEngine.Core
{
    public sealed class FrameTimer
    {
        private readonly System.Diagnostics.Stopwatch _stopwatch = System.Diagnostics.Stopwatch.StartNew();
        private double _lastTime;

        public double DeltaTime { get; private set; }
        public double TotalTime { get; private set; }

        public void Tick()
        {
            var current = _stopwatch.Elapsed.TotalSeconds;
            DeltaTime = current - _lastTime;
            TotalTime = current;
            _lastTime = current;
        }
    }
}
