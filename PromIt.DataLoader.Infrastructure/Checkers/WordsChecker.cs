using PromIt.DataLoader.Console.Infrastructure.Extensions;
using PromIt.DataLoader.Infrastructure.Readers;
using System.Text;

namespace PromIt.DataLoader.Infrastructure.Checkers
{
    /// <summary>
    /// Класс с методами валидации слов.
    /// </summary>
    internal class WordsChecker
    {
        /// <summary>
        /// Опции работы <see cref="WordsReader"/>
        /// </summary>
        private readonly WordsReaderOptions options;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public WordsChecker(WordsReaderOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Возвращает признак валидности слова.
        /// </summary>
        public bool IsValid(StringBuilder word)
        {
            if (word.Length < 1)
            {
                return false;
            }

            if (NeedCheckOption(WordsReaderOptions.WordHasAtLeast2Vowels))
            {
                var vowelsAmount = 0;
                for (var i = 0; i < word.Length && vowelsAmount < 2; i++) 
                {
                    if (word[i].IsVowel())
                    {
                        vowelsAmount++;
                    }
                }
                if (vowelsAmount < 2)
                {
                    return false;
                }
            }

            if (NeedCheckOption(WordsReaderOptions.WordLengthIsLessOrEquals400Chars))
            {
                if (word.Length > 400)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Валидирует сформированный словарь слов.
        /// </summary>
        public void Validate(IDictionary<string, int> words)
        {
            if (!NeedCheckOption(WordsReaderOptions.WordIsContainedAtLeast3Times))
            {
                return;
            }
            
            foreach(var key in words.Keys)
            {
                if (words[key] < 3)
                {
                    words.Remove(key);
                }
            }
        }


        private bool NeedCheckOption(WordsReaderOptions option)
            => (options & option) == option;
    }
}