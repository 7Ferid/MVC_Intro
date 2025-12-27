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


    }
}
