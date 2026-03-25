using System.Collections.Concurrent;

namespace AstraEngine.Threading
{
    public sealed class JobQueue
    {
        private readonly ConcurrentQueue<Job> _jobs = new();

        public int Count => _jobs.Count;

        public void Enqueue(Job job)
        {
            _jobs.Enqueue(job);
        }

        public bool TryDequeue(out Job job)
        {
            return _jobs.TryDequeue(out job);
        }
    }
}
