namespace PromIt.DataSearcher.Console.Helpers
{
    /// <summary>
    /// Класс-хелпер для работы с <see cref="System.Console" />.
    /// </summary>
    internal static class ConsoleHelper
    {
        /// <summary>
        /// Пишест строку в консоль в указанном цвете.
        /// </summary>
        public static void WriteColoredLine(string message, ConsoleColor color)
        {
            var oldColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldColor;
        }
    }
}
