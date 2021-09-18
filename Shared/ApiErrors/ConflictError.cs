using System.Net;

namespace Shared.ApiErrors
{
    public class ConflictError : ApiError
    {
        public ConflictError() : base(409, HttpStatusCode.Conflict.ToString())
        {
        }

        public ConflictError(string message) : base(409, HttpStatusCode.Conflict.ToString(), message)
        {
        }
    }
}