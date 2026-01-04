
using MVC_Intro.Models.Common;

namespace MVC_Intro.Models
{
    public class Tag : Basentity
    {
        public string Name { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }
}
