using System;
using Shared.Enums;

namespace Shared.Dto
{
    public class UserDto : UserProfileDto
    {
        public string UserId { get; set; }

        public Language Language { get; set; }
    }
}