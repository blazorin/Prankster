using System.ComponentModel.DataAnnotations;

namespace Model.Data
{
    public class Package
    {
        public string PackageId { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public float Price { get; set; }
        [Required] public float Calls { get; set; }

        [Required] public bool IsInAnOffer { get; set; } = false;
    }
}