using PromIt.DataLoader.Console.Infrastructure.Loaders;
using PromIt.DataLoader.Console.Infrastructure.Readers;

namespace PromIt.DataLoader.Tests
{
    [TestFixture]
    public class DbDataLoaderTests
    {
        private DataLoader<string> loader;

        [SetUp]
        public void Setup()
        {
            var reader = new WordsReader(WordsReaderOptions.WordHasAtLeast2Vowels
                | WordsReaderOptions.WordLengthIsLessOrEquals400Chars
                | WordsReaderOptions.WordIsContainedAtLeast3Times);

            //dbWordsLoader = new DataDbLoader(reader);
        }

        [Test]
        public async Task DbDataLoaderTest()
        {
            var files = Directory.GetFiles("Files");
            //await dbWordsLoader.LoadAsync(files);
        }
    }
}
