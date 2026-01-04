using Microsoft.EntityFrameworkCore;
using MVC_Intro.Models;

namespace MVC_Intro.Contexts
{
    public class AppDbContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-G5VLGHT\\SQLEXPRESS;Database=MVCIntroDb;Trusted_Connection=true;TrustServerCertificate=true");
        //    base.OnConfiguring(optionsBuilder);
        //}
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }



    }
}
