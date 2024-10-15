using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        /*
         modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);
         */
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Enrollment>().HasKey(e=> new {e.SewingCourseId ,e.ApplicationUserId});
            builder.Entity<Enrollment>().HasOne(e => e.ApplicationUser)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(u => u.ApplicationUserId);

            builder.Entity<Enrollment>().HasOne(e => e.SewingCourse)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(u => u.SewingCourseId);


            base.OnModelCreating(builder);


        }
        public DbSet<RefreshTocken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomClothingOrder> customClothingOrders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
       public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SewingCourse> SewingCourses { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
