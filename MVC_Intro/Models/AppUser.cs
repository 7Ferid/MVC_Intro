using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace MVC_Intro.Models
{
    public class AppUser:IdentityUser
    {
        public String FullName { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; } = [];

    }
}
