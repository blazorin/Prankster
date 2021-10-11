using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using JetBrains.Annotations;
using Shared.Enums;
using Shared.Utils;

namespace Model.Data
{
    public class User
    {
#pragma warning disable CS8618
        public string UserId { get; set; }

        [Required, StringLength(FieldLenghts.User.Identifier)]
        public string Identifier { get; set; }

        // phone number sometimes may be set as SIM
        [Required] public IdentifierType IdentifierType { get; set; }

        // 4-digit support pin and is also an additional auth validation code
        [Required] public int Pin { get; set; }


        [StringLength(FieldLenghts.User.Mail)] public string? Email { get; set; }

        public List<Device> DeviceModels { get; set; }
        public List<IpAddress> IPAddresses { get; set; }

        [Required] public Platform LastPlatform { get; set; }
        public string OSVersion { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastLogin { get; set; }

        [StringLength(FieldLenghts.User.Country)]
        public string? Country { get; set; }

        public Language Language { get; set; }

        public float CallBalance { get; set; }

        public List<Refer> Refers { get; set; }
        public bool IsReferred { get; set; }
        public bool IsBanned { get; set; }
        public bool IsPremium { get; set; }
        public bool TermsAccepted { get; set; } = true;

        // User endpoint
        public bool EndpointCreated { get; set; }
        public string? EndpointUsername { get; set; }

        public List<UserPerms> Perms { get; set; }
        //public List<string> EnabledTokens { get; set; }
        //public List<string> DisabledTokens { get; set; }

        public List<UserLog> Logs { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Call> Calls { get; set; }
        public List<Prank> LikedPranks { get; set; }
    }
}