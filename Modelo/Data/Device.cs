using JetBrains.Annotations;

namespace Model.Data
{
    public class Device
    {
        public string DeviceId { get; set; }

        [CanBeNull] public string DeviceModel { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}