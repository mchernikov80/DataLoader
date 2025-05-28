using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Database;

namespace PromIt.DataLoader.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            
            var config = builder.Build();
            var connectionString = config.GetConnectionString("ApplicationDbContext");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            using (var db = new ApplicationDbContext(options))
            {
                var words = db.LoadedWords.ToList();
            }


            System.Console.Read();
        }
    }
}