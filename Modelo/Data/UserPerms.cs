using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Data
{
    public class UserPerms
    {
        public string UserPermsId { get; set; }

        [Required] public string PermKey { get; set; }

        [Required] public string PermValue { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}