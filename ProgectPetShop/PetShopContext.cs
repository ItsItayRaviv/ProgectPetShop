using ProgectPetShop.Models;
using System.Data.Entity;

namespace ProgectPetShop
{
    public class PetShopContext : DbContext
    {
        public DbSet<Animal>? Animals { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Comment>? Comments { get; set; }
    }
}
