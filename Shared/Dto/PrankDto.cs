using Shared.Enums;

namespace Shared.Dto
{
    public class PrankDto
    {
        public string PrankId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Premium { get; set; }
        public Language Language { get; set; }
        public float Popularity { get; set; }
        public string ImageName { get; set; }
    }
}