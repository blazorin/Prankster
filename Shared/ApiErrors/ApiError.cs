namespace Shared.ApiErrors
{
    public class ApiError
    {
        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string Message { get; set; }

        public ApiError(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;

#pragma warning disable CS8601 // Possible null reference assignment.
            Message = statusCode switch
            {
                401 => "unauthorized",
                403 => "no_permission",
                404 => "not_found",
                429 => "too_many_requests",
                _ => Message
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public ApiError(int statusCode, string statusDescription, string message) : this(statusCode, statusDescription)
        {
            this.Message = message;
        }
    }
}