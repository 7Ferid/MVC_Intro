using MVC_Intro.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.Models
{
    public class Product : Basentity
    {
        public string Name { get; set; }
     
        public string? Description { get; set; }
     
        public decimal Price { get; set; }
      

        public Category Category { get; set; }
      
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
