using System;

namespace Shared.Dto
{
    public class AdminUserDto : UserDto
    {
        public DateTime LastLogin { get; set; }
        public bool IsBanned { get; set; }
        public bool TermsAccepted { get; set; }
        public string Identifier { get; set; }
    }
}