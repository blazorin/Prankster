using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Data;
using Model.Services;
using Shared;
using Shared.ApiErrors;
using Shared.Dto;
using Google.Apis.Auth;
using Model.Enums;
using Shared.Enums;
using Server.Secure;
using Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Model.Extensions;
using System.Text.Json;


namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("pranks")]
    public class PranksController : ControllerBase
    {
        private readonly IPrankServices _prankServices;

        public PranksController(IPrankServices prankServices) => _prankServices = prankServices;


        [HttpGet("fetch")]
        public async Task<IActionResult> FetchTrendingAndLatest()
        {
            IEnumerable<PrankDto> trending = await _prankServices.GetTrendingPranks();
            IEnumerable<PrankDto> latest = await _prankServices.GetLatestPranks();

            if (trending is not null && latest is not null)
                return Ok(new HomePranksDto() { Trending = trending, Latest = latest });

            return Conflict(new ConflictError("fetch_pranks_error"));
        }

        [HttpGet("fetch/{prankId}")]
        public async Task<IActionResult> FetchPrank(int prankId)
        {
            if (prankId is 0 or > 1000)
                return Conflict(new ConflictError("fetch_prank_error"));

            PrankDto prankObj = await _prankServices.GetPrankById(prankId);
            
           if (prankObj is null)
                return Conflict(new ConflictError("fetch_prank_error"));

           return Ok(prankObj);

        }
    }
}
