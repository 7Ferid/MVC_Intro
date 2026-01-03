using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.ViewModels.ProductViewModels
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }


        [Required]
        public int CategoryId { get; set; }

        public IFormFile MainImage { get; set; }
        public IFormFile HoverImage { get; set; }


        [Required]
        [Range(0, 5)]
        public int ReytingCount { get; set; }   
        public List<IFormFile>? Images { get; set; }
    }

}


