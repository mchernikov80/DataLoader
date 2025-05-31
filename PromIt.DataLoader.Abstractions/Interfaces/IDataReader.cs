namespace PromIt.DataLoader.Abstractions.Interfaces
{
    /// <summary>
    /// Интерфейс считывателя данных указанного типа.
    /// </summary>
    /// <typeparam name="T">Тип считываемых данных.</typeparam>
    public interface IDataReader<T>
    {
        /// <summary>
        /// Считывает данные.
        /// </summary>
        public Task<IDictionary<T, int>> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
