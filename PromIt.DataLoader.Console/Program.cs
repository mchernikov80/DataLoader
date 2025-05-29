using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Console.Infrastructure.Loaders;
using PromIt.DataLoader.Console.Infrastructure.Readers;
using PromIt.DataLoader.Database;

namespace PromIt.DataLoader.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var dbConnectionString = configuration.GetConnectionString("ApplicationDbContext");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseSqlServer(dbConnectionString).Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var wordsReaderOptions = WordsReaderOptions.WordHasAtLeast2Vowels
                    | WordsReaderOptions.WordLengthIsLessOrEquals400Chars
                    //| WordsReaderOptions.WordIsContainedAtLeast3Times
                    ;

                var reader = new WordsReader(wordsReaderOptions);
                var dataLoader = new DataDbLoader(dbContext, reader);
                await dataLoader.LoadAsync(@"d:\Projects\Prom-IT\PromIt.DataLoader\PromIt.DataLoader.Tests\Files\test3.txt");
                
            }
        }
    }
}