using MVC_Intro.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.Models
{
    public class Product : Basentity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
      

        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string MainImagePath { get; set; }


        public string HoverImagePath { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = [];
        public ICollection<ProductTag> ProductTags { get; set; } = [];
        [Required]
        [Range(0,5)]
        public int ReytingCount { get; set; }

    }
}
