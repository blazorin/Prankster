using System.Net;

namespace Shared.ApiErrors
{
    public class NotFoundError : ApiError
    {
        public NotFoundError() : base(404, HttpStatusCode.NotFound.ToString())
        {
        }


        public NotFoundError(string message)
            : base(404, HttpStatusCode.NotFound.ToString(), message)
        {
        }
    }
}