using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Console.Infrastructure.Loaders;
using PromIt.DataLoader.Console.Infrastructure.Readers;
using PromIt.DataLoader.Console.Infrastructure.Uploaders;
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

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var wordsReaderOptions = WordsReaderOptions.WordHasAtLeast2Vowels
                | WordsReaderOptions.WordLengthIsLessOrEquals400Chars
                //| WordsReaderOptions.WordIsContainedAtLeast3Times
                ;
            var reader = new WordsReader(wordsReaderOptions);
            using (var uploader = new WordsDbUploader(configuration))
            {
                var wordsLoader = new DataLoader<string>(reader, uploader);
                await wordsLoader.LoadAsync(files);
            }

            stopwatch.Stop();

            System.Console.WriteLine($"Data Loader: загрузка данных завершена - {stopwatch}");
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