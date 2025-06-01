using Microsoft.Extensions.Configuration;
using Moq;
using PromIt.DataLoader.Infrastructure.Loaders;
using PromIt.DataLoader.Infrastructure.Readers;
using PromIt.DataLoader.Infrastructure.Uploaders;

namespace PromIt.DataLoader.Tests
{
    [TestFixture]
    public class DbDataLoaderTests
    {
        private DataLoader<string> loader;

        private int loadedWordsAmount;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var options = WordsReaderOptions.WordHasAtLeast2Vowels
                | WordsReaderOptions.WordLengthIsLessOrEquals400Chars;
                //| WordsReaderOptions.WordIsContainedAtLeast3Times;

            var reader = new WordsReader(options);

            loadedWordsAmount = 0;

            var uploader = new Mock<WordsDbUploader>(configuration);

            uploader.Setup(e => e.UploadDbAsync(It.IsAny<IDictionary<string, int>>(), CancellationToken.None))
                .Returns<IDictionary<string, int>, CancellationToken>((loadData, cancellationToken) =>
                {
                    loadedWordsAmount += loadData.Values.Sum();
                    return Task.CompletedTask;
                });


            loader = new DataLoader<string>(reader, uploader.Object);
        }

        [Test]
        public async Task DbDataLoaderTest()
        {
            var files = new string[] { @"Files\test3.txt" };

            var tasks = new List<Task>();
            for (var i = 0; i < 50; i++)
            {
                tasks.Add(loader.LoadAsync(files));
            }
            
            await Task.WhenAll(tasks);
            Assert.That(loadedWordsAmount, Is.EqualTo(5250));
        }
    }
}
