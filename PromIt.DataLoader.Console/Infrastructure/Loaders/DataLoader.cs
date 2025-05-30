using PromIt.DataLoader.Console.Infrastructure.Readers;
using PromIt.DataLoader.Abstractions.Interfaces;

namespace PromIt.DataLoader.Console.Infrastructure.Loaders
{
    /// <summary>
    /// Загрузчик данных в БД.
    /// </summary>
    public class DataLoader
    {
        private readonly WordsReader dataReader;

        private readonly IDataUploader<string> dataUploader;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataLoader(WordsReader dataReader, IDataUploader<string> dataUploader)
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
