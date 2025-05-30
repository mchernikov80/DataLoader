using PromIt.DataLoader.Abstractions.Interfaces;

namespace PromIt.DataLoader.Infrastructure.Loaders
{
    /// <summary>
    /// Загрузчик данных.
    /// </summary>
    public class DataLoader<T>
    {
        private readonly IDataReader<T> dataReader;

        private readonly IDataUploader<T> dataUploader;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataLoader(IDataReader<T> dataReader, IDataUploader<T> dataUploader)
        {
            this.dataReader = dataReader;
            this.dataUploader = dataUploader;
        }

        public async Task LoadAsync(IEnumerable<string> files, CancellationToken cancellationToken = default)
        {
            foreach (var file in files)
            {
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    var loadData = await dataReader.ReadAsync(stream, cancellationToken);
                    await dataUploader.UploadAsync(loadData, cancellationToken);
                }
            }
        }
    }
}
