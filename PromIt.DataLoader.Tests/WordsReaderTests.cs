using PromIt.DataLoader.Infrastructure.Readers;

namespace PromIt.DataLoader.Tests
{
    [TestFixture]
    public class WordsReaderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task WordsReaderTest()
        {
            using (var stream = new FileStream(@"Files\Тестовое_задание_2_Текстовый_процессор.txt", FileMode.Open))
            {
                var reader = new WordsReader();
                var words = await reader.ReadAsync(stream);
            }
        }

        [Test]
        public async Task WordsReaderTest1()
        {
            using (var stream = new FileStream(@"Files\test1.txt", FileMode.Open))
            {
                var reader = new WordsReader();
                var words = await reader.ReadAsync(stream);

                Assert.That(words.Values.Sum(), Is.EqualTo(27));
                Assert.IsTrue(words.ContainsKey("приложение"));
                Assert.IsTrue(words.ContainsKey("параметра"));
            }
        }

        [Test]
        public async Task WordsReaderTest2()
        {
            using (var stream = new FileStream(@"Files\test2.txt", FileMode.Open))
            {
                var reader = new WordsReader();
                var words = await reader.ReadAsync(stream);

                Assert.That(words.Values.Sum(), Is.EqualTo(93));
                Assert.IsTrue(words.ContainsKey("C#"));
                Assert.IsTrue(words.ContainsKey("NET"));
                Assert.IsTrue(words.ContainsKey("7"));
                Assert.IsTrue(words.ContainsKey("UTF-8"));
                Assert.IsTrue(words.ContainsKey("2-ух"));
                Assert.IsTrue(words.ContainsKey("3-ёх"));
                Assert.That(4, Is.EqualTo(words["данных"]));
            }
        }

        [TestCase(WordsReaderOptions.None)]
        [TestCase(WordsReaderOptions.WordHasAtLeast2Vowels)]
        [TestCase(WordsReaderOptions.WordLengthIsLessOrEquals400Chars)]
        [TestCase(WordsReaderOptions.WordIsContainedAtLeast3Times)]
        [TestCase(WordsReaderOptions.WordHasAtLeast2Vowels | WordsReaderOptions.WordLengthIsLessOrEquals400Chars)]
        [TestCase(WordsReaderOptions.WordHasAtLeast2Vowels | WordsReaderOptions.WordLengthIsLessOrEquals400Chars | WordsReaderOptions.WordIsContainedAtLeast3Times)]
        public async Task WordsReaderTest3(WordsReaderOptions options)
        {
            using (var stream = new FileStream(@"Files\test3.txt", FileMode.Open))
            {
                var reader = new WordsReader(options);
                var words = await reader.ReadAsync(stream);

                if (options == WordsReaderOptions.None)
                {
                    Assert.That(words.Values.Sum(), Is.EqualTo(108));
                }
                else if (options == WordsReaderOptions.WordHasAtLeast2Vowels)
                {
                    Assert.That(words.Values.Sum(), Is.EqualTo(106));
                }
                else if (options == WordsReaderOptions.WordLengthIsLessOrEquals400Chars)
                {
                    Assert.That(words.Values.Sum(), Is.EqualTo(107));
                }
                else if (options == WordsReaderOptions.WordIsContainedAtLeast3Times)
                {
                    Assert.That(words.Values.Sum(), Is.EqualTo(100));
                }
                else if ((options & WordsReaderOptions.WordHasAtLeast2Vowels) == WordsReaderOptions.WordHasAtLeast2Vowels
                    && (options & WordsReaderOptions.WordLengthIsLessOrEquals400Chars) == WordsReaderOptions.WordLengthIsLessOrEquals400Chars
                    && (options & WordsReaderOptions.WordIsContainedAtLeast3Times) != WordsReaderOptions.WordIsContainedAtLeast3Times)
                {
                    Assert.That(words.Values.Sum(), Is.EqualTo(105));
                }
                else if ((options & WordsReaderOptions.WordHasAtLeast2Vowels) == WordsReaderOptions.WordHasAtLeast2Vowels
                    && (options & WordsReaderOptions.WordLengthIsLessOrEquals400Chars) == WordsReaderOptions.WordLengthIsLessOrEquals400Chars
                    && (options & WordsReaderOptions.WordIsContainedAtLeast3Times) == WordsReaderOptions.WordIsContainedAtLeast3Times)
                {
                    Assert.That(words.Values.Sum(), Is.EqualTo(100));
                }
            }
        }
    }
}