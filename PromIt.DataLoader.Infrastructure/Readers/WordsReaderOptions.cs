namespace PromIt.DataLoader.Infrastructure.Readers
{
    /// <summary>
    /// Перечисление с опциями работы считывателя слов.
    /// </summary>
    [Flags]
    public enum WordsReaderOptions
    {
        /// <summary>
        /// Любое слово.
        /// </summary>
        None = 0,

        /// <summary>
        /// Слово содержит не менее 2-ух гласные букв.
        /// </summary>
        WordHasAtLeast2Vowels = 1,

        /// <summary>
        /// Слово имеет длину не более 400 символов.
        /// </summary>
        WordLengthIsLessOrEquals400Chars = 2,

        /// <summary>
        /// Слово упоминается в текущем входном файле не менее 3-ёх раз.
        /// </summary>
        WordIsContainedAtLeast3Times = 4
    }
}