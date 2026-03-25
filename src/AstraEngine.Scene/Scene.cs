namespace AstraEngine.Scene
{
    public sealed class Scene
    {
        private readonly List<ISceneObject> _objects = [];

        public Camera Camera { get; } = new();
        public LightSet Lights { get; } = new();
        public IReadOnlyList<ISceneObject> Objects => _objects;

        public void Add(ISceneObject obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            _objects.Add(obj);
        }

        public bool Remove(ISceneObject obj) => _objects.Remove(obj);

        public void Update(float deltaTime)
        {
            foreach (var obj in _objects)
                obj.Update(deltaTime);
        }
    }
}
