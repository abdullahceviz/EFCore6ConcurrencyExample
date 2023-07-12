using Microsoft.EntityFrameworkCore;

namespace ConcurrencyExample.Models
{
    public class MssqlDbContext:DbContext
    {
        //DbContextOptionsu program.cs dosyasından gönderebilmek için parametreli bir constructur tanımladık
        //bu dbcontext optionsu DbContext sınıfının parametreli constructurına gönderiyorruz
        public MssqlDbContext(DbContextOptions<MssqlDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API
            modelBuilder.Entity<Product>().Property(x => x.Version).IsRowVersion();
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);    
            base.OnModelCreating(modelBuilder);
        }
    }
}
