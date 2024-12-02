using BP_215UniqloMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.DataAccess
{
    public class UniqloDbContext:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public  DbSet<ProductImage> ProductImage { get; set; }

        public UniqloDbContext(DbContextOptions opt):base(opt) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(
                x=>x.Property(y=>y.CreatedTime)
                .HasDefaultValueSql("GetDate()"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
