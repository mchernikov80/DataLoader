using PromIt.DataLoader.Abstractions.Interfaces;
using PromIt.DataLoader.Console.Infrastructure.Extensions;
using PromIt.DataLoader.Infrastructure.Checkers;
using System.Text;

namespace PromIt.DataLoader.Infrastructure.Readers
{
    /// <summary>
    /// Считыватель слов.
    /// </summary>
    public class WordsReader : IDataReader<string>
    {
        /// <summary>
        /// Опции работы.
        /// </summary>
        private readonly WordsReaderOptions options;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public WordsReader() : this(WordsReaderOptions.None)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public WordsReader(WordsReaderOptions options)
        {
            this.options = options;
        }

        /// <inheritdoc/>
        public async Task<IDictionary<string, int>> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            var words = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var wordsChecker = new WordsChecker(options);

            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    var line = await reader.ReadLineAsync(cancellationToken)
                        .ConfigureAwait(false);
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    foreach (var word in GetWords(line, wordsChecker))
                    {
                        if (!words.ContainsKey(word))
                        {
                            words.Add(word, 1);
                            continue;
                        }

                        words[word]++;
                    }
                }
            }
            wordsChecker.Validate(words);
            return words;
        }

        private IEnumerable<string> GetWords(string line, WordsChecker wordsChecker)
        {
            var word = new StringBuilder();
            
            foreach (var @char in line)
            {
                if (@char.IsLetterOrDigit() || (@char.IsSpecial() && word.Length > 0))
                {
                    word.Append(@char);
                    continue;
                }

                if (word.Length > 0)
                {
                    if (wordsChecker.IsValid(word))
                    {
                        yield return word.ToString();
                    }
                    word.Clear();
                }
            }

            if (wordsChecker.IsValid(word))
            {
                yield return word.ToString();
            }
        }
    }
}