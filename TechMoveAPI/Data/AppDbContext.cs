using Microsoft.EntityFrameworkCore;
using TechMoveAPI.Models;

/*
Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
Date: 18 February 2023
Date Accessed: 10 April 2026
*/
namespace TechMoveAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

        public DbSet<Client> Clients { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/modeling/
        Date: 28 March 2023
        Date Accessed: 10 April 2026
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship: Client - Contracts (1..*)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: ContractEntity - ServiceRequests (1..*)
            modelBuilder.Entity<ContractEntity>()
                .HasMany(c => c.ServiceRequests)
                .WithOne(sr => sr.Contract)
                .HasForeignKey(sr => sr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.Cost)
                .HasPrecision(18, 2);

                modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.OriginalAmount)
                .HasPrecision(18, 2);
        }
    }
}