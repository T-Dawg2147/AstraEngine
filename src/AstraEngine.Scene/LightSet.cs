namespace AstraEngine.Scene
{
    public sealed class LightSet
    {
        private readonly List<Light> _lights = [];

        public IReadOnlyList<Light> Lights => _lights;

        public void Add(Light light)
        {
            ArgumentNullException.ThrowIfNull(light);
            _lights.Add(light);
        }

        public bool Remove(Light light) => _lights.Remove(light);

        public void Clear() => _lights.Clear();
    }
}
