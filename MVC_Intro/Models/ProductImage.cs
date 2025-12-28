using MVC_Intro.Models.Common;

namespace MVC_Intro.Models
{
    public class ProductImage : Basentity
    {
      public int ProductdId { get; set; } 
        public Product Product { get; set; }
        public string ImagePath { get; set; }
      

    }
}
