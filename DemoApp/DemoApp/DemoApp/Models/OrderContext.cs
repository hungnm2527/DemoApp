using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }

        public List<Customer> GetCustomers() => Customer.ToList();

        public List<Order> GetOrders() => Order.ToList();

        public List<Product> GetProducts() => Product.ToList();

        public List<Category> GetCategories() => Category.ToList();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(opc => new { opc.OrderID, opc.ProductID, opc.CustomerID });

            modelBuilder.Entity<Order>()
                .HasOne(opc => opc.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(opc => opc.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(opc => opc.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(opc => opc.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(pc => pc.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
