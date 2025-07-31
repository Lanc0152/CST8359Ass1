using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Data
{
    public class VetSystemDbContext : DbContext
    {
        
            public VetSystemDbContext(DbContextOptions<VetSystemDbContext> options) : base(options)
            {
            }

            public DbSet<VetDoctor> VetDoctors { get; set; }
            public DbSet<Pet> Pets { get; set; }
            public DbSet<PetDetails> PetDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<VetDoctor>().ToTable("vetDoctors");
                modelBuilder.Entity<Pet>().ToTable("pets");
                modelBuilder.Entity<PetDetails>().ToTable("petDetails");
        }
    }
    
}
