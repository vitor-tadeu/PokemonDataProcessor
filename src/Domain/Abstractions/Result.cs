namespace Domain.Abstractions
{
    public class Result<T>
    {
        public T? Data { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private Result(T? data, bool isSuccess, string errorMessage)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T data) => new(data, true, string.Empty);
        public static Result<T> Failure(string errorMessage) => new(default, false, errorMessage);
    }
}
