using MVC_Intro.Models.Common;

namespace MVC_Intro.Models
{
    public class BasketItem:Basentity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Count { get; set; }

        public AppUser AppUser { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
    }
}
