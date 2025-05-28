using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Database.Entities;

namespace PromIt.DataLoader.Database
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<LoadedWord> LoadedWords { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=localhost;Database=PromIT.LoadedData;Trusted_Connection=True;");
        }
    }
}