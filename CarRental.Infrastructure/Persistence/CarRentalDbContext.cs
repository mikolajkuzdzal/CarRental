using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CarRental.Infrastructure.Persistence
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Rentals)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Rentals)
                .WithOne(r => r.Car)
                .HasForeignKey(r => r.CarId);
        }
    }
}
