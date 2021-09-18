namespace Model.Data
{
    public class Refer
    {
        public string ReferId { get; set; }
        public string ReferedUserId { get; set; }


        public User User { get; set; }
        public string UserId { get; set; }
    }
}