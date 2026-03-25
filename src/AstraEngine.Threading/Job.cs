namespace AstraEngine.Threading
{
    public readonly struct Job
    {
        public Job(Action action)
        {

        }

        public Action Action { get; }

        public void Execute()
        {
            Action();
        }
    }
}
