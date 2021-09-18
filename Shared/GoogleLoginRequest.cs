using Shared.Dto;

namespace Shared
{
    public class GoogleLoginRequest : UserLoginDto
    {
        public string IdToken { get; set; }
    }
}