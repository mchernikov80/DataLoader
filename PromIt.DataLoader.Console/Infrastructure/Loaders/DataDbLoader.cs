using Microsoft.EntityFrameworkCore;
using PromIt.DataLoader.Console.Infrastructure.Extensions;
using PromIt.DataLoader.Console.Infrastructure.Readers;
using PromIt.DataLoader.Database;
using PromIt.DataLoader.Database.Entities;

namespace PromIt.DataLoader.Console.Infrastructure.Loaders
{
    /// <summary>
    /// Загрузчик данных в БД.
    /// </summary>
    public class DataDbLoader
    {
        private readonly ApplicationDbContext dbContext;
        private readonly WordsReader reader;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataDbLoader(ApplicationDbContext dbContext, WordsReader reader)
        {
            this.dbContext = dbContext;
            this.reader = reader;
        }

        public async Task LoadAsync(params string[] files)
        {
            foreach (var file in files)
            {
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    var loadData = await reader.ReadAsync(stream);
                    var dbUpdatedStatistics = await UpdateDbAsync(loadData);
                    if (dbUpdatedStatistics.AddedRows > 0)
                    {
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        private async Task<(int AddedRows, int UpdatedRows)> UpdateDbAsync(IDictionary<string, int> loadData)
        {
            var addedDbRows = 0;
            var updatedDbRows = 0;

            foreach (var word in loadData.Keys)
            {
                var amount = loadData[word];
                var updatedRows = await dbContext.LoadedWords
                    .Where(e => e.Word == word)
                    .ExecuteUpdateAsync(s => s.SetProperty(e => e.Amount, e => e.Amount + amount));
                updatedDbRows += updatedRows;

                if (updatedRows == 0)
                {
                    var loadedWord = new LoadedWord { Word = word, Amount = amount };
                    await dbContext.AddOrUpdateAsync(loadedWord);
                    addedDbRows++;
                }
            }
            return (addedDbRows, updatedDbRows);
        }
    }
}
