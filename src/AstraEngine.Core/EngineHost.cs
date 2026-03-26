using System.Diagnostics;

namespace AstraEngine.Core
{
    public sealed class EngineHost : IDisposable
    {
        private bool _isRunning;
        private bool _disposed;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private double _lastTime;

        public EngineHost(EngineConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Time = new EngineTime(0, 0, 0);
        }

        public EngineConfig Config { get; }

        public EngineTime Time { get; private set; }

        public void Run(IGameApplication application)
        {
            ArgumentNullException.ThrowIfNull(application);

            if (_disposed)
                throw new ObjectDisposedException(nameof(EngineHost));

            if (_isRunning)
                throw new InvalidOperationException("EngineHost is already running.");

            _isRunning = true;

            Logger.Info($"Starting {Config.AppName}...");
            application.Initialize(this);

            _lastTime = _stopwatch.Elapsed.TotalSeconds;

            while (_isRunning)
            {
                var currentTime = _stopwatch.Elapsed.TotalSeconds;
                var unscaledDelta = currentTime - _lastTime;
                _lastTime = currentTime;

                var delta = Config.TargetFrameRate > 0.0
                    ? Math.Min(unscaledDelta, 1.0 / Config.TargetFrameRate * 4.0)
                    : unscaledDelta;

                Time = new EngineTime(delta, currentTime, unscaledDelta);

                application.Update(Time);

                if (Config.TargetFrameRate > 0.0)
                {
                    var targetFrameTime = 1.0 / Config.TargetFrameRate;
                    var frameElapsed = _stopwatch.Elapsed.TotalSeconds - currentTime;
                    var remaining = targetFrameTime - frameElapsed;

                    if (remaining > 0)
                    {
                        var sleepMs = (int)(remaining * 1000.0);
                        if (sleepMs > 1)
                            Thread.Sleep(1);
                        else if (sleepMs > 0)
                            Thread.Sleep(0);
                    }
                }
            }

            application.Shutdown();
            Logger.Info("Engine stopped.");
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _stopwatch.Stop();
        }
    }

    public interface IGameApplication
    {
        void Initialize(EngineHost host);
        void Update(in EngineTime time);
        void Shutdown();
    }
}
