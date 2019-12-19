using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class _DbContext: IdentityDbContext
    {
        public _DbContext(DbContextOptions<_DbContext> options, IConfiguration config): base (options)
        {
            
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        //public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<RegisterCode> RegisterCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne<Product>(op => op.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne<Order>(op => op.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<Product>()
                .Property(p => p.CategoryId)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentMethodId)
                .IsRequired();

            //modelBuilder.Entity<OrderProduct>()
            //    .Property(p => p.)
            //    .IsRequired();

            modelBuilder.Entity<OrderProduct>()
                .Property(p => p.ProductId)
                .IsRequired();

            //modelBuilder.Entity<OrderProduct>()
            //    .Property(p => p.Order)
            //    .IsRequired();

            modelBuilder.Entity<OrderProduct>()
                .Property(p => p.OrderId)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(p => p.PaymentId)
                .IsRequired();

            modelBuilder.Entity<Image>()
                .Property(p => p.ProductId)
                .IsRequired();


        }
    }
}
