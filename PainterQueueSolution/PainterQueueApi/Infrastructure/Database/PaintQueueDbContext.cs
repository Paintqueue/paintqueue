using Microsoft.EntityFrameworkCore;
using PainterQueueApi.Models;

namespace PainterQueueApi.Infrastructure.Database
{
    /// <summary>
    /// Represents the database context for the PainteQueue application, providing access to the Users and Rules
    /// collections.
    /// </summary>
    /// <remarks>This class is derived from <see cref="DbContext"/> and is used to interact with the
    /// underlying database.</remarks>
    public class PaintQueueDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the collection of users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the collection of rules stored in the database.
        /// </summary>
        public DbSet<Rule> Rules { get; set; }

        public PaintQueueDbContext(DbContextOptions<PaintQueueDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Users table
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);


            // Rules table
            modelBuilder.Entity<Rule>()
                .HasKey(r => r.Id);
            
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Useful for debugging, but can expose sensitive data, so don’t enable in production.
            // optionsBuilder.EnableSensitiveDataLogging(); 
            optionsBuilder.EnableDetailedErrors();
        }
    }
}
