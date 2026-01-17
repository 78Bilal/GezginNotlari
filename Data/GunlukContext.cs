using Microsoft.EntityFrameworkCore;
using SeyahatGunlugu.Models;

namespace SeyahatGunlugu.Data
{
    public class GunlukContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
           optionsBuilder.UseSqlite("Data Source=./wwwroot/GünlükListesi.db");
       }
        public DbSet<GunlukModel> Gunlukler { get; set; }
    }
}
