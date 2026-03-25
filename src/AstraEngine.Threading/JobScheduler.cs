namespace AstraEngine.Threading
{
    public sealed class JobScheduler : IDisposable
    {
        private readonly JobQueue _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly List<Task> _workers = [];
        private readonly int _workerCount;

        public JobScheduler(int? workerCount = null)
        {
            _workerCount = workerCount ?? Math.Max(1, Environment.ProcessorCount - 1);

            for (var i = 0; i < _workerCount; i++)
                _workers.Add(Task.Run(WorkerLoop, _cts.Token));
        }

        public void Enqueue(Action action)
        {
            _queue.Enqueue(new Job(action));
        }

        public void Enqueue(Job job)
        {
            _queue.Enqueue(job);
        }

        public void Flush()
        {
            while (_queue.TryDequeue(out var job))
                job.Execute();
        }

        public void Dispose()
        {
            _cts.Cancel();

            try
            {
                Task.WaitAll(_workers.ToArray());
            }
            catch { }

            _cts.Dispose();
        }

        private async Task WorkerLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var job))
                {
                    try
                    {
                        job.Execute();
                    }
                    catch { }                    
                }
                else
                {
                    await Task.Delay(1, _cts.Token).ConfigureAwait(false);
                }
            }
        }
    }
}
