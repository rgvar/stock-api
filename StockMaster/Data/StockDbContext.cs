using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StockMaster.Entities;
using StockMaster.Models;
using StockMaster.Users;

namespace StockMaster.Data
{
    public class StockDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }

        public StockDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPC for Order
            modelBuilder.Entity<Order>().UseTpcMappingStrategy();
            modelBuilder.Entity<SalesOrder>().ToTable("SalesOrders");
            modelBuilder.Entity<PurchaseOrder>().ToTable("PurchaseOrders");

            // TPC for Contact
            modelBuilder.Entity<Contact>().UseTpcMappingStrategy();
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Supplier>().ToTable("Suppliers");


            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(p => p.Subtotal)
                    .HasColumnType("decimal(18,2)");
                entity.Property(p => p.Total)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");
            });

            // Set ID to ProductOrder
            modelBuilder.Entity<ProductOrder>()
                .HasKey(po => new { po.OrderId, po.ProductId });

            // One to Many: ProductOrder <<--> Order
            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // One to Many: ProductOrder <<--> Product
            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Product)
                .WithMany()
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // One to One: Order <--> Invoice
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.SetNull);


            // Many to Many:  Products <<-->> Categories
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products);


            // One to Many: Client <-->> SalesOrders
            modelBuilder.Entity<Client>()
                .HasMany(c => c.SalesOrders)
                .WithOne(so => so.Client)
                .HasForeignKey(so => so.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            // One to Many: Supplier <-->> PurchaseOrders
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.PurchaseOrders)
                .WithOne(po => po.Supplier)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);


            // One to Many: Invoice --> PaymentType
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.PaymentType)
                .WithMany()
                .HasForeignKey(i => i.PaymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<PaymentType>().HasData(
                    new PaymentType() { Id = 1, Name = "Cash", Short = "C" },
                    new PaymentType() { Id = 2, Name = "CreditCard", Short = "CC" },
                    new PaymentType() { Id = 3, Name = "DebitCard", Short = "DC" },
                    new PaymentType() { Id = 4, Name = "MobilePayment", Short = "MP" }
            );


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.UpdatedAt = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
