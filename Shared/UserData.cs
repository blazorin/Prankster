using System.Collections.Generic;
using System.Security.Claims;
using Shared.Enums;

namespace Shared
{
    public class UserData
    {
        public IEnumerable<string> Roles { get; set; }
        public Dictionary<string, string> Policies { get; set; }
        public string Token { get; set; }
        // it may have changed from ComplexIdentifier, if it already existed
        public string FinalIdentifier { get; set; }

        public float CallBalance { get; set; }
        public Language Language { get; set; }
    }
}