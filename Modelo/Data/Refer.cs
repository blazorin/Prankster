namespace Model.Data
{
    public class Refer
    {
#pragma warning disable CS8618
        public string ReferId { get; set; }
        public string ReferedUserId { get; set; }


        public User User { get; set; }
        public string UserId { get; set; }
    }
}