using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromIt.DataLoader.Console.DataReaders
{
    internal class WordChecker
    {
        private static HashSet<char> vowelsSet = new HashSet<char>("aeiouAEIOUаоыиуэёяеюАОЫИУЭЁЯЕЮ");

        private readonly StringBuilder word;

        private readonly WordsReaderOptions options;

        private int vowelCharsCount = 0;

        private bool HasAtLeast2Vowels => vowelCharsCount >= 2;

        private bool IsLessOrEquals400Chars => word.Length <= 400;

        public WordChecker(StringBuilder word, WordsReaderOptions options) 
        {
            this.word = word;
            this.options = options;
        }

        //internal void Check(WordsReaderCheckOptions option)
        //{
        //    if (!NeedCheckOption(option))
        //    {
        //        return;
        //    }

        //    if (option == WordsReaderCheckOptions.WordHasAtLeast2Vowels && !HasAtLeast2Vowels)
        //    {
        //        if (vowelsSet.Contains(word[word.Length - 1]))
        //        {
        //            vowelCharsCount++;
        //        }
        //        return HasAtLeast2Vowels;
        //    }

        //    return true;
        //}

        public void Clear()
        {
            vowelCharsCount = 0;
        }
        

        internal void VowelsCheck()
        {
            if (word.Length < 1)
            {
                return;
            }

            if (NeedCheckOption(WordsReaderOptions.WordHasAtLeast2Vowels) && !HasAtLeast2Vowels)
            {
                if (vowelsSet.Contains(word[word.Length - 1]))
                {
                    vowelCharsCount++;
                }
            }
        }

        internal bool IsValid()
        {
            if (word.Length < 1)
            {
                return false;
            }

            if (NeedCheckOption(WordsReaderOptions.WordHasAtLeast2Vowels) && !HasAtLeast2Vowels)
            {
                return false;
                
            }

            if (NeedCheckOption(WordsReaderOptions.WordIsLessOrEquals400Chars) && !IsLessOrEquals400Chars)
            {
                return false;
            }

            //if (NeedCheckOption(WordsReaderOptions.WordIsContainedAtLeast3Times))
            //{

            //}

            return true;
        }

        private bool NeedCheckOption(WordsReaderOptions option)
            => (options & option) == option;


    }
}
