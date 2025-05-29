using Microsoft.EntityFrameworkCore;
using PromIt.DataLoader.Database;
using PromIt.DataLoader.Database.Entities;

namespace PromIt.DataLoader.Infrastructure.Extensions
{
    /// <summary>
    /// Класс с extension-методами для типа <see cref="ApplicationDbContext">System.Char</see>.
    /// </summary>
    public static class ApplicationDbContextExtension
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

        public async static Task<IEnumerable<LoadedWord>> FindWordsAsync(this ApplicationDbContext dbContext, (string Word, int Amount) filter, int take = 10)
        {
            var query = dbContext.LoadedWords
                .Where(e => EF.Functions.Like(e.Word, $"{filter.Word}%"));
            
            if (filter.Amount > 0)
            {
                query = query.Where(e => e.Amount >= filter.Amount);
            }

            var words = await query
                .OrderByDescending(e => e.Amount)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);

            return words;
        }
    }
}