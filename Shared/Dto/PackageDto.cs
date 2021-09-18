namespace Shared.Dto
{
    public class PackageDto
    {
        public string PackageId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Calls { get; set; }

        public bool IsInAnOffer { get; set; }
    }
}