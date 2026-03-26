namespace AstraEngine.Core
{
    public readonly struct Result<T>
    {
        public Result(T? value, string? error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        public static Result<T> Success(T value) => new(value, null, true);
        public static Result<T> Fail(string error) => new(default, error, false);
    }
}