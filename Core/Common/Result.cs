namespace Core.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(T? data, bool isSuccess, string? error) : base(isSuccess, error)
        {
            Data = data;
        }

        public static Result<T> Success(T data) => new(data, true, null);

        public static new Result<T> Failure(string error) => new(default, false, error);
    }
}
