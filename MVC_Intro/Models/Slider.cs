using MVC_Intro.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.Models
{
    public class Slider:Basentity
    {
      
       
        [Required(ErrorMessage ="Logo bos ola bilmez")]
        public string LogoUrl { get; set; }=null!;
        [MaxLength(20,ErrorMessage ="maksimum uzunlugu 20 dir ")]
        [MinLength(3,ErrorMessage ="Minimum uzunlugu 3 dur")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;

    }
}
