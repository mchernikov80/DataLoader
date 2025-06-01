using PromIt.DataLoader.Abstractions.Interfaces;

namespace PromIt.DataLoader.Infrastructure.Loaders
{
    /// <summary>
    /// Загрузчик данных.
    /// </summary>
    public class DataLoader<T>
    {
        /// <summary>
        /// Считыватель данных.
        /// </summary>
        private readonly IDataReader<T> dataReader;

        /// <summary>
        /// Загрузчик данных.
        /// </summary>
        private readonly IDataUploader<T> dataUploader;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataLoader(IDataReader<T> dataReader, IDataUploader<T> dataUploader)
        {
            this.dataReader = dataReader;
            this.dataUploader = dataUploader;
        }

        /// <summary>
        /// Загрузить данные из файлов.
        /// </summary>
        public async Task LoadAsync(IEnumerable<string> files, CancellationToken cancellationToken = default)
        {
            foreach (var file in files)
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var loadData = await dataReader.ReadAsync(stream, cancellationToken);
                    await dataUploader.UploadAsync(loadData, cancellationToken);
                }
            }
        }
    }
}
