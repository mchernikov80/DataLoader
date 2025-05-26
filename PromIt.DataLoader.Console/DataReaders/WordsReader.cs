using PromIt.DataLoader.Console.DataReaders;
using System.Text;

namespace PromIt.DataLoader.DataReader
{
    public class WordsReader: IDisposable
    {
        private readonly Stream stream;

        /// <summary>
        /// Connstructor.
        /// </summary>
        public WordsReader(Stream stream)
        {
            this.stream = stream;
        }

        #region IDisposable implementation
        private bool disposed = false;

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;

            try
            {
                stream?.Close();
            }
            catch
            {
            }
        }
        #endregion

        public async Task<IDictionary<string, int>> ReadAsync(WordsReaderOptions options = WordsReaderOptions.None, CancellationToken cancellationToken = default)
        {
            var words = new Dictionary<string, int>();

            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    var line = await reader.ReadLineAsync(cancellationToken);
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    foreach (var word in GetWords(line, options))
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

            return words;
        }

        //public IDictionary<string, int> ReadData(Stream stream)
        //{
        //    var words = new Dictionary<string, int>();

        //    using (var reader = new StreamReader(stream))
        //    {
        //        while (reader.Peek() >= 0)
        //        {
        //            var line = reader.ReadLine();
        //            if (string.IsNullOrEmpty(line))
        //            {
        //                continue;
        //            }

        //            foreach (var word in GetWords(line, options))
        //            {
        //                if (!words.ContainsKey(word))
        //                {
        //                    words.Add(word, 1);
        //                    continue;
        //                }

        //                words[word]++;
        //            }
        //        }
        //    }

        //    return words;
        //}

        private IEnumerable<string> GetWords(string line, WordsReaderOptions options)
        {
            var text = line.AsSpan();

            var word = new StringBuilder();
            var wordChecker = new WordChecker(word, options);

            var words = new List<string>();

            for (var charIndex = 0; charIndex < text.Length; charIndex++)
            {
                var @char = text[charIndex];
                if (char.IsLetterOrDigit(@char)
                    || (IsSpecialChar(@char) && word.Length > 0))
                {
                    word.Append(@char);
                    wordChecker.VowelsCheck();
                    continue;
                }
                
                if (wordChecker.IsValid())
                {
                    words.Add(word.ToString());
                }

                if (word.Length > 0)
                {
                    word.Clear();
                    wordChecker.Clear();
                }
            }

            if (wordChecker.IsValid())
            {
                words.Add(word.ToString());
            }

            return words;
        }

        private static bool IsSpecialChar(char sign)
        {
            const string specialChar = @"#$%&@£§€-_";
            return specialChar.Contains(sign);
 
        }
    }
}