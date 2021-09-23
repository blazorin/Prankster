using JetBrains.Annotations;
using Shared.Enums;
using System.Collections.Generic;

namespace Shared.Dto
{
    public class UserLoginDto
    {
        /*
         * Identifier
         * Android -> SIM
         * iOS -> Phone Number
         */
        public List<string> ComplexIdentifier { get; set; }

        // simpleIdentifier
        public string Identifier { get; set; }
        public int Pin { get; set; }
        public Platform LastPlatform { get; set; }

        [CanBeNull] public string DeviceModel { get; set; }

        [CanBeNull] public string OSVersion { get; set; }

        public string IPAddress { get; set; }

        // email is filled if comes from oauth
        public string Email { get; set; }
    }
}