using System;

namespace Shared.Dto
{
    public class UserProfileDto
    {
        //filled if user has a linked google account
        public string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsPremium { get; set; }

        public float CallBalance { get; set; }
    }
}