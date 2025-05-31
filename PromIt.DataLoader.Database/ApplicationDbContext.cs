using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Database.Entities;


namespace PromIt.DataLoader.Database
{
    /// <summary>
    /// EF контекст базы данных.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        ///  Набор свойств конфигурации приложения.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ApplicationDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            Database.EnsureCreated();
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbConnectionString = configuration.GetConnectionString("ApplicationDbContext");
            var options = optionsBuilder.UseSqlServer(dbConnectionString).Options;
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LoadedWord>(entity =>
            {
                entity.HasIndex(e => e.Word).IsUnique();
            });
        }

        /// <summary>
        /// Загруженные слова.
        /// </summary>
        public DbSet<LoadedWord> LoadedWords { get; set; } = null!;
    }
}