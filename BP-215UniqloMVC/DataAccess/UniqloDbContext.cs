using BP_215UniqloMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace BP_215UniqloMVC.DataAccess
{
    public class UniqloDbContext:IdentityDbContext<User>
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

        //internal async Task SaveChangesAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
