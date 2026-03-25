namespace AstraEngine.Core
{
    public readonly struct EngineTime
    {
        public EngineTime(double deltaTime, double totalTime, double unscaledDeltaTime)
        {
            DeltaTime = deltaTime;
            TotalTime = totalTime;
            UnscaledDeltaTime = unscaledDeltaTime;
        }

        public double DeltaTime { get; }
        public double TotalTime { get; }
        public double UnscaledDeltaTime { get; }
    }
}
