using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Database;
using PromIt.DataLoader.Infrastructure.Extensions;
using PromIt.DataSearcher.Console.Helpers;

namespace PromIt.DataSearcher.Console
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

                var filterText = string.Empty;
                while (true)
                {
                    System.Console.WriteLine("Введите фильтр поиска: слово и, через пробел, кол-во его упоминаний(опционально). Для завершения ввода, нажмите клавишу Enter (выход-пустая строка):");
                    filterText = System.Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(filterText))
                    {
                        break;
                    }

                    if (filterText.Length == 1 && char.IsControl(filterText, 0))
                    {
                        ConsoleHelper.WriteColoredLine("Критерии поиска указаны неверно", ConsoleColor.Red);
                        continue;
                    }

                    ExtractFilters(filterText, out var word, out var amount);
                    if (string.IsNullOrEmpty(word))
                    {
                        ConsoleHelper.WriteColoredLine("Критерии поиска указаны неверно", ConsoleColor.Red);
                        continue;
                    }
                    
                    var findedWords = await dbContext.FindWordsAsync(filter: new(word!, amount));
                    foreach(var findedWord in findedWords)
                    {
                        ConsoleHelper.WriteColoredLine($"{findedWord.Word} - {findedWord.Amount}", ConsoleColor.Green);
                    }

                    System.Console.WriteLine();
                }
            }
        }

        private static void ExtractFilters(string filterText, out string? word, out int amount)
        {
            word = null;
            amount = -1;

            var splited = filterText.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (splited.Length == 0)
            {
                return;
            }

            word = splited[0];
            if (splited.Length > 1 && int.TryParse(splited[1], out var result))
            {
                amount = result;
            }
        }
    }
}