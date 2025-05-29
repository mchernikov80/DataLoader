using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Console.Infrastructure.Loaders;
using PromIt.DataLoader.Console.Infrastructure.Readers;
using PromIt.DataLoader.Database;
using System.Diagnostics;

namespace PromIt.DataLoader.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var files = GetFiles(args);
            if (!files.Any()) 
            { 
                return;
            }

            System.Console.WriteLine($"Data Loader: старт");

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
                var dataLoader = new DataDbLoader(configuration, dbContext, reader);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                await dataLoader.LoadAsync(files);
                stopwatch.Stop();

                System.Console.WriteLine($"Data Loader: загрузка данных завершена - {stopwatch}");
            }

            System.Console.ReadKey();
        }

        private static IEnumerable<string> GetFiles(IEnumerable<string> paths)
        {
            var files = new List<string>();
            foreach (var path in paths) 
            {
                var attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    files.AddRange(Directory.GetFiles(path));
                    continue;
                }
                files.Add(path);
            }

            return files;
        }
    }
}