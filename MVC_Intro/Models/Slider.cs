using MVC_Intro.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Intro.Models
{
    public class Slider:Basentity
    {
      
       
        //[Required(ErrorMessage ="Logo bos ola bilmez")]
        public string? LogoUrl { get; set; }
        [NotMapped]
        public IFormFile Logo { get; set; }

        [MaxLength(20,ErrorMessage ="maksimum uzunlugu 20 dir ")]
        [MinLength(3,ErrorMessage ="Minimum uzunlugu 3 dur")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;

    }
}
