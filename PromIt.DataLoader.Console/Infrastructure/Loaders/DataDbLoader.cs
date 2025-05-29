using Microsoft.EntityFrameworkCore;
using PromIt.DataLoader.Infrastructure.Extensions;
using PromIt.DataLoader.Console.Infrastructure.Readers;
using PromIt.DataLoader.Database;
using PromIt.DataLoader.Database.Entities;
using Microsoft.Extensions.Configuration;

namespace PromIt.DataLoader.Console.Infrastructure.Loaders
{
    /// <summary>
    /// Загрузчик данных в БД.
    /// </summary>
    public class DataDbLoader
    {
        private readonly Semaphore updateDbSemaphore;

        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly WordsReader reader;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataDbLoader(IConfiguration configuration, ApplicationDbContext dbContext, WordsReader reader)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.reader = reader;


            var semaphoreName = configuration.GetConnectionString("UpdateDbSemaphoreName");
            updateDbSemaphore = new Semaphore(1, 1, semaphoreName);
        }

        public async Task LoadAsync(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    var loadData = await reader.ReadAsync(stream);

                    updateDbSemaphore.WaitOne();
                    try
                    {
                        var dbUpdatedStatistics = await UpdateDbAsync(loadData);
                        if (dbUpdatedStatistics.AddedRows > 0)
                        {
                            await dbContext.SaveChangesAsync()
                                .ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        updateDbSemaphore.Release();
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
                    .ExecuteUpdateAsync(s => s.SetProperty(e => e.Amount, e => e.Amount + amount))
                    .ConfigureAwait(false);
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
