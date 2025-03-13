namespace MercerStore.Web.Infrastructure.Helpers
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? ErrorMessage { get; }

        private Result(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        private Result(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T data) => new(data);
        public static Result<T> Failure(string errorMessage) => new(errorMessage);
    }
}
