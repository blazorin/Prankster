using System;
using Shared.Enums;

namespace Model.Data
{
    public class UserLog
    {
        public string UserLogId { get; set; }
        public UserLogType UserLogType { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}