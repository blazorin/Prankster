using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.ApiErrors;

namespace Server.Controllers
{
    [ApiController]
    [Route("errors")]
    public class ErrorsController : ControllerBase
    {
        [Route("{code}")]
        public Task<ObjectResult> Error(int code)
        {
            HttpStatusCode parsedCode = (HttpStatusCode)code;
            ApiError error = new ApiError(code, parsedCode.ToString());

            return Task.FromResult(new ObjectResult(error));
        }
    }
}