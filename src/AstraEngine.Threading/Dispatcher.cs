using System.Collections.Concurrent;

namespace AstraEngine.Threading
{
    public sealed class Dispatcher
    {
        private readonly ConcurrentQueue<Action> _actions = new();

        public void Post(Action action)
        {
            _actions.Enqueue(action ?? throw new ArgumentNullException(nameof(action)));
        }

        public void Pump()
        {
            while (_actions.TryDequeue(out var action))
            {
                try
                {
                    action();
                }
                catch { }
            }
        }
    }
}
