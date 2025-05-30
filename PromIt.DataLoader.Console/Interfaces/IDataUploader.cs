namespace PromIt.DataLoader.Console.Interfaces
{
    public interface IDataUploader<T>
    {
        Task UploadAsync(IDictionary<T, int> loadData, CancellationToken cancellationToken = default);
    }
}
