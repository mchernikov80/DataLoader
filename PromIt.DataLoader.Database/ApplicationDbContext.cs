using Microsoft.EntityFrameworkCore;
using PromIt.DataLoader.Database.Entities;


namespace PromIt.DataLoader.Database
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<LoadedWord> LoadedWords { get; set; } = null!;
    }
}