using Microsoft.EntityFrameworkCore;
using PromIt.DataLoader.Database;
using PromIt.DataLoader.Database.Entities;

namespace PromIt.DataLoader.Console.Infrastructure.Extensions
{
    /// <summary>
    /// Класс с extension-методами для типа <see cref="ApplicationDbContext">System.Char</see>.
    /// </summary>
    internal static class ApplicationDbContextExtension
    {
        public async static Task AddOrUpdateAsync(this ApplicationDbContext dbContext, LoadedWord loadedWord)
        {
            if (loadedWord == null) 
            {
                throw new ArgumentNullException(nameof(loadedWord)); 
            }

            if (loadedWord.Id == 0)
            {
                await dbContext.LoadedWords.AddAsync(loadedWord)
                    .ConfigureAwait(false);
                return;
            }

            var loadedWordDb = await dbContext.LoadedWords
                .SingleAsync(e => e.Id == loadedWord.Id)
                .ConfigureAwait(false);

            loadedWordDb.Word = loadedWord.Word;
            loadedWordDb.Amount += loadedWord.Amount;
        }
    }
}