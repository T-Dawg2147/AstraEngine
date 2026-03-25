namespace AstraEngine.Core
{
    public sealed class FixedStepTimer
    {
        private double _accumulator;

        public FixedStepTimer(double stepSeconds)
        {
            StepSeconds = stepSeconds;
        }

        public double StepSeconds { get; }
        public int StepsThisFrame { get; private set; }

        public void Tick(double deltaTime)
        {
            StepsThisFrame = 0;
            _accumulator += deltaTime;

            while (_accumulator >= StepSeconds)
            {
                _accumulator -= StepSeconds;
                StepsThisFrame++;
            }
        }
    }
}
