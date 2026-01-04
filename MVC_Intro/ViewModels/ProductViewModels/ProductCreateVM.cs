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
        public List<int> TagIds { get; set; }

        public IFormFile MainImage { get; set; } = null!;
        public IFormFile HoverImage { get; set; } = null!;


        [Required]
        [Range(0, 5)]
        public int ReytingCount { get; set; }
        public List<IFormFile> Images { get; set; } = [];
    }

}


