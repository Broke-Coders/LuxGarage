namespace LuxGarage.API
{
    /// <summary>
    /// Represents a standardized API response structure for the LuxGarage API, containing properties for indicating success, status code, 
    /// message, data, errors, and timestamp, and providing static methods for creating various types of responses such as OK, 
    /// Created, NoContent, BadRequest, NotFound, Conflict, and Error responses, ensuring consistent and informative API responses 
    /// across the application.
    /// </summary>
    /// <typeparam name="TData">The type of the data contained in the response.</typeparam>
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }

        public object? Errors { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// Creates a new instance of the ApiResponse class with the specified properties.
        /// </summary>
        /// <param name="success">Indicates whether the request was successful.</param>
        /// <param name="statusCode">The HTTP status code for the response.</param>
        /// <param name="message">A message describing the response.</param>
        /// <param name="data">The data contained in the response.</param>
        /// <param name="errors">Any errors associated with the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default,
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

        /// <summary>
        /// Creates a successful API response with the provided data and message, 
        /// setting the success property to true and the status code to 200 (OK).
        /// </summary>
        /// <param name="data">The data contained in the response.</param>
        /// <param name="message">A message describing the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> Ok(TData data, string message) =>
            Create(success: true, statusCode: 200, message: message, data: data);

        /// <summary>
        /// Creates a successful API response with the provided data and message, 
        /// setting the success property to true and the status code to 201 (Created),
        /// indicating that a new resource has been created successfully.
        /// </summary>
        /// <param name="data">The data contained in the response.</param>
        /// <param name="message">A message describing the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> CreatedAt(TData data, string message) =>
            Create(success: true, statusCode: 201, message: message, data: data);

        /// <summary>
        /// Creates a successful API response with the provided message, 
        /// setting the success property to true and the status code to 204 (No Content), indicating that the request was successful but 
        /// there is no content to return in the response body, often used for delete operations or when an update is successful 
        /// without returning the updated resource.
        /// </summary>
        /// <param name="message">A message describing the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> NoContent(string message = "Operation completed successfully.")
            => Create(success: true, statusCode: 204, message: message);

        /// <summary>
        /// Creates a bad request API response with the provided message and optional errors, 
        /// setting the success property to false and the status code to 400 (Bad Request),
        /// indicating that the request was invalid or cannot be processed, 
        /// often used when there are validation errors or when the client sends incorrect data.
        /// </summary>
        /// <param name="message">A message describing the response.</param>
        /// <param name="errors">Any errors associated with the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> BadRequest(string message, object? errors = null) =>
            Create(success: false, statusCode: 400, message: message, errors: errors);

        /// <summary>
        /// Creates a not found API response with the provided message and optional errors,
        /// setting the success property to false and the status code to 404 (Not Found),
        /// indicating that the requested resource could not be found, often used when a client requests a 
        /// resource that does not exist or has been deleted, providing a clear message to the client about the missing resource.
        /// </summary>
        /// <param name="message">A message describing the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> NotFound(string message = "Resource not found.") =>
            Create(success: false, statusCode: 404, message: message);

        /// <summary>
        /// Creates a conflict API response with the provided message,
        /// setting the success property to false and the status code to 409 (Conflict),
        /// indicating that the request could not be processed due to a conflict with the current state of the resource.
        /// </summary>
        /// <param name="message">A message describing the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> Conflict(string message) =>
            Create(success: false, statusCode: 409, message: message);

        /// <summary>
        /// Creates an error API response with the provided status code, message, and optional errors,
        /// setting the success property to false.
        /// </summary>
        /// <param name="statusCode">The HTTP status code for the response.</param>
        /// <param name="message">A message describing the response.</param>
        /// <param name="errors">Any errors associated with the response.</param>
        /// <returns>A new instance of the ApiResponse class.</returns>
        public static ApiResponse<TData> Error(int statusCode, string message, object? errors = null) =>
            Create(success: false, statusCode: statusCode, message: message, errors: errors);
    }
}
