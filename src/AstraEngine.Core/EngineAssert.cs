namespace AstraEngine.Core
{
    public static class EngineAssert
    {
        public static void IsTrue(bool condition, string? message = null)
        {
            if (!condition)
                throw new InvalidOperationException(message ?? "Assertion failed: condition was false.");
        }

        public static void IsNotNull(object? value, string? message = null)
        {
            if (value is null)
                throw new InvalidOperationException(message ?? "Assertion failed: value was null.");
        }
    }
}
