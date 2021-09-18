using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace Model.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        /// <summary>
        /// Obtener ID a través del ClaimsPrincipal del usuario
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException"></exception>
        public static string GetId(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new AuthenticationException($"Required {ClaimTypes.NameIdentifier} claims not found");

            if (Guid.TryParse(claim.Value, out Guid id))
            {
                return id.ToString();
            }


            throw new AuthenticationException($"Invalid claim {ClaimTypes.NameIdentifier} value: {claim.Value}");
        }
    }
}