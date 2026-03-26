namespace AstraEngine.Threading
{
    public readonly struct Job
    {
        public Job(Action action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public Action Action { get; }

        public void Execute()
        {
            Action();
        }
    }
}
