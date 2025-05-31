namespace PromIt.DataLoader.Abstractions.Interfaces
{
    /// <summary>
    /// Интерфейс загрузчика данных указанного типа.
    /// </summary>
    /// <typeparam name="T">Тип загружаемых данных.</typeparam>
    public interface IDataUploader<T>
    {
        /// <summary>
        /// Загружает данные.
        /// </summary>
        Task UploadAsync(IDictionary<T, int> loadData, CancellationToken cancellationToken = default);
    }
}
