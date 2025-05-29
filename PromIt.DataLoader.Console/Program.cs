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

            using (var dbContext = new ApplicationDbContext(configuration))
            {
                var wordsReaderOptions = WordsReaderOptions.WordHasAtLeast2Vowels
                    | WordsReaderOptions.WordLengthIsLessOrEquals400Chars
                    //| WordsReaderOptions.WordIsContainedAtLeast3Times
                    ;

                var reader = new WordsReader(wordsReaderOptions);
                var dataLoader = new DataDbLoader(dbContext, reader);
                await dataLoader.LoadAsync(@"d:\Projects\Prom-IT\PromIt.DataLoader\PromIt.DataLoader.Tests\Files\test3.txt",
                    @"d:\Projects\Prom-IT\PromIt.DataLoader\Тестовое_задание_2_Текстовый_процессор.txt");
            }
        }
    }
}