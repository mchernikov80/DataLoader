namespace PromIt.DataLoader.Abstractions.Interfaces
{
    public interface IDataReader<T>
    {
        public Task<IDictionary<T, int>> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
