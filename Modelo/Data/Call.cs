using System;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Model.Data
{
    public class Call
    {
        public string CallId { get; set; }
        [Required] public CallStatus Status { get; set; }
        [Required] public Prank Prank { get; set; }
        [Required] public DateTime DateRequested { get; set; }

        // only filled if scheduled
        public DateTime DateScheduled { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}