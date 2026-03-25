namespace AstraEngine.Core
{
    /// <summary>
    /// Opaque typed handle used for resources and entities
    /// </summary>
    public readonly struct Handle<T> : IEquatable<Handle<T>>
    {
        public Handle(uint id)
        {

        }

        public uint Id { get; }

        public bool IsValid => Id != 0;

        public bool Equals(Handle<T> other) => Id == other.Id;
        public override bool Equals(object? obj) => obj is Handle<T> other && Equals(other);
        public override int GetHashCode() => (int)Id;

        public static bool operator ==(Handle<T> left, Handle<T> right) => left.Equals(right);
        public static bool operator !=(Handle<T> left, Handle<T> right) => !left.Equals(right);

        public override string ToString() => IsValid ? $"{typeof(T).Name}:{Id}" : $"{typeof(T).Name}:Invalid";
    }
}
