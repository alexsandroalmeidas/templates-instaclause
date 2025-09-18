namespace Templates.Api.Infrastructure.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public IEnumerable<string>? Errors { get; init; }
        public object? Meta { get; init; }

        public static ApiResponse<T> Ok(T data, object? meta = null) =>
            new() { Success = true, Data = data, Meta = meta };

        public static ApiResponse<T> Fail(string error) =>
            new() { Success = false, Errors = new[] { error } };

        public static ApiResponse<T> Fail(IEnumerable<string> errors) =>
            new() { Success = false, Errors = errors };
    }
}
