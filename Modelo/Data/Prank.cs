using System;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Model.Data
{
    public class Prank
    {
        public string PrankId { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }

        [Required] public bool Premium { get; set; } = false;
        [Required] public bool OnlyAdmin { get; set; } = false;
        [Required] public bool Enabled { get; set; } = true;
        [Required] public string AudioFileName { get; set; }
        [Required] public Language Language { get; set; }
        public float Popularity { get; set; }
        [Required] public DateTime DateAdded { get; set; }
    }
}