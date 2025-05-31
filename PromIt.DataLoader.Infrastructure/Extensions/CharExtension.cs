namespace PromIt.DataLoader.Console.Infrastructure.Extensions
{
    /// <summary>
    /// Класс с extension-методами для типа <see cref="char" />.
    /// </summary>
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

        /// <summary>
        /// Возвращает признак гласной в значении символа.
        /// </summary>
        public static bool IsVowel(this char @char) => vowelsSet.Contains(@char);

        /// <summary>
        /// Возвращает признак специальных символов, которые могут присутствовать в словах.
        /// </summary>
        public static bool IsSpecial(this char @char) => specialCharSet.Contains(@char);

        /// <summary>
        /// Возвращает признак литеры или цифры в значении символа.
        /// </summary>
        public static bool IsLetterOrDigit(this char @char) => char.IsLetterOrDigit(@char);
    }
}
