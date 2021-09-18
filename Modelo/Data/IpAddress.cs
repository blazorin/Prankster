namespace Model.Data
{
    public class IpAddress
    {
        public string IpAddressId { get; set; }

        public string Value { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}