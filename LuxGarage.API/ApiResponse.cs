namespace LuxGarage.API
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }

        public object? Errors { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData data = default,
            object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }


        public static ApiResponse<TData> Ok(TData data, string message) =>
            Create(success: true, statusCode: 200, message: message, data: data);

        public static ApiResponse<TData> CreatedAt(TData data, string message) =>
            Create(success: true, statusCode: 201, message: message, data: data);

        public static ApiResponse<TData> NoContent(string message = "Operation completed successfully.")
            => Create(success: true, statusCode: 204, message: message);

        public static ApiResponse<TData> BadRequest(string message, object? errors = null) =>
            Create(success: false, statusCode: 400, message: message, errors: errors);
        public static ApiResponse<TData> NotFound(string message = "Resource not found.") =>
            Create(success: false, statusCode: 404, message: message);

        public static ApiResponse<TData> Conflict(string message) =>
            Create(success: false, statusCode: 409, message: message);

        public static ApiResponse<TData> Error(int statusCode, string message, object? errors = null) =>
            Create(success: false, statusCode: statusCode, message: message, errors: errors);
    }
}
