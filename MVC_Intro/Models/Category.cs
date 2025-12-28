using MVC_Intro.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.Models
{
    public class Category : Basentity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } 
        
    }
}
