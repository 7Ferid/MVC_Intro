
using MVC_Intro.Models.Common;

namespace MVC_Intro.Models
{
    public class ProductTag:Basentity
    {  
        public Product Product { get; set; }= null!;
        public int ProductId { get; set; }
            
        public Tag Tag { get; set; }= null!;

        public int TagId { get; set; }
    }
}
