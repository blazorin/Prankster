using JetBrains.Annotations;
using Shared.Enums;

namespace Shared.Dto
{
    public class UserLoginDto
    {
        /*
         * Identifier
         * Android -> SIM
         * iOS -> Phone Number
         */
        public string Identifier { get; set; }
        public Platform LastPlatform { get; set; }

        [CanBeNull] public string DeviceModel { get; set; }
        public string IPAddress { get; set; }

        // email is filled if comes from oauth
        public string Email { get; set; }
    }
}