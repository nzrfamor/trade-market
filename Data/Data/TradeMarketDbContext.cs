using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Person)
                .WithOne()
                .HasForeignKey<Customer>(c => c.PersonId);
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Receipts)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);
            modelBuilder.Entity<Receipt>()
                .HasMany(r => r.ReceiptDetails)
                .WithOne(rd => rd.Receipt)
                .HasForeignKey(rd => rd.ReceiptId);
            modelBuilder.Entity<ReceiptDetail>()
                .HasKey(x => new { x.ReceiptId, x.ProductId });
            modelBuilder.Entity<ProductCategory>()
                .HasMany(pc => pc.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.ProductCategoryId);
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ReceiptDetails)
                .WithOne(rd => rd.Product)
                .HasForeignKey(rd => rd.ProductId);
        }
    }
}
