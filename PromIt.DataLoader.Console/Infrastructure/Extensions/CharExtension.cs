using System;

namespace PromIt.DataLoader.Console.Infrastructure.Extensions
{
    public static class CharExtension
    {
        /// <summary>
        /// Набор гласных в латинице и кириллице.
        /// </summary>
        private static HashSet<char> vowelsSet = new HashSet<char>("aeiouAEIOUаоыиуэёяеюАОЫИУЭЁЯЕЮ");

        /// <summary>
        /// Набор специальных символов.
        /// </summary>
        private static HashSet<char> specialCharSet = new HashSet<char>(@"#$%&@£§€-_");

        public static bool IsVowel(this char @char) => vowelsSet.Contains(@char);

        public static bool IsSpecial(this char @char) => specialCharSet.Contains(@char);

        public static bool IsLetterOrDigit(this char @char) => char.IsLetterOrDigit(@char);
    }
}
