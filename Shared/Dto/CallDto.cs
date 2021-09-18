using System;
using Shared.Enums;

namespace Shared.Dto
{
    public class CallDto
    {
        public string CallId { get; set; }

        public CallStatus Status { get; set; }
        public PrankDto Prank { get; set; }

        public DateTime DateRequested { get; set; }
        public DateTime DateScheduled { get; set; }
    }
}