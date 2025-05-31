using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PromIt.DataLoader.Abstractions.Interfaces;
using PromIt.DataLoader.Database;
using PromIt.DataLoader.Database.Entities;
using PromIt.DataLoader.Infrastructure.Extensions;

namespace PromIt.DataLoader.Infrastructure.Uploaders
{
    /// <summary>
    /// Загрузчик слов в БД.
    /// </summary>
    public class WordsDbUploader : IDataUploader<string>, IDisposable
    {
        /// <summary>
        /// Семафор работы загрузчика.
        /// </summary>
        private readonly Semaphore updateDbSemaphore;

        /// <summary>
        /// EF контекст базы данных.
        /// </summary>
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public WordsDbUploader(IConfiguration configuration) 
        { 
            var semaphoreName = configuration.GetConnectionString("UpdateDbSemaphoreName");
            updateDbSemaphore = new Semaphore(1, 1, semaphoreName);
            dbContext = new ApplicationDbContext(configuration);
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            disposed = true;

            try
            {
                updateDbSemaphore?.Release();
            }
            catch { }

            try
            {
                updateDbSemaphore?.Close();
            }
            catch { }

            try
            {
                dbContext?.Dispose();
            }
            catch { }
        }

        ~WordsDbUploader()
        {
            Dispose(disposing:false);
        }
        #endregion

        /// <inheritdoc />
        public async Task UploadAsync(IDictionary<string, int> loadData, CancellationToken cancellationToken = default)
        {
            updateDbSemaphore.WaitOne();
            try
            {
                var dbUpdatedStatistics = await UpdateDbAsync(loadData, cancellationToken);
                if (dbUpdatedStatistics.AddedRows > 0)
                {
                    await dbContext.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                updateDbSemaphore.Release();
            }
        }

        private async Task<(int AddedRows, int UpdatedRows)> UpdateDbAsync(IDictionary<string, int> loadData, CancellationToken cancellationToken)
        {
            var addedDbRows = 0;
            var updatedDbRows = 0;

            foreach (var word in loadData.Keys)
            {
                cancellationToken.ThrowIfCancellationRequested();

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
