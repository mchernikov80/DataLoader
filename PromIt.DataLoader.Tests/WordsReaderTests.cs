using PromIt.DataLoader.Console.DataReaders;
using PromIt.DataLoader.DataReader;
using System.Text.Unicode;

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
                using (var reader = new WordsReader(stream))
                {
                    var words = await reader.ReadAsync();
                }
            }
        }

        [Test]
        public async Task WordsReaderTest1()
        {
            using (var stream = new FileStream(@"Files\test1.txt", FileMode.Open))
            {
                using (var reader = new WordsReader(stream))
                {
                    var words = await reader.ReadAsync();

                    Assert.That(27, Is.EqualTo(words.Values.Sum()));
                    Assert.IsTrue(words.ContainsKey("приложение"));
                    Assert.IsTrue(words.ContainsKey("параметра"));
                }
            }
        }

        [Test]
        public async Task WordsReaderTest2()
        {
            using (var stream = new FileStream(@"Files\test2.txt", FileMode.Open))
            {
                using (var reader = new WordsReader(stream))
                {
                    var words = await reader.ReadAsync();

                    Assert.That(93, Is.EqualTo(words.Values.Sum()));
                    Assert.IsTrue(words.ContainsKey("C#"));
                    Assert.IsTrue(words.ContainsKey("NET"));
                    Assert.IsTrue(words.ContainsKey("7"));
                    Assert.IsTrue(words.ContainsKey("UTF-8"));
                    Assert.IsTrue(words.ContainsKey("2-ух"));
                    Assert.IsTrue(words.ContainsKey("3-ёх"));
                    Assert.That(4, Is.EqualTo(words["данных"]));
                }
            }
        }

        [TestCase(WordsReaderOptions.None)]
        [TestCase(WordsReaderOptions.WordHasAtLeast2Vowels)]
        [TestCase(WordsReaderOptions.WordIsLessOrEquals400Chars)]
        [TestCase(WordsReaderOptions.WordHasAtLeast2Vowels | WordsReaderOptions.WordIsLessOrEquals400Chars)]
        public async Task WordsReaderTest3(WordsReaderOptions options)
        {
            using (var stream = new FileStream(@"Files\test3.txt", FileMode.Open))
            {
                using (var reader = new WordsReader(stream))
                {
                    var words = await reader.ReadAsync(options);

                    if (options == WordsReaderOptions.None)
                    {
                        Assert.That(words.Values.Sum(), Is.EqualTo(108));
                    }
                    else if ((options & WordsReaderOptions.WordHasAtLeast2Vowels) == WordsReaderOptions.WordHasAtLeast2Vowels
                        && (options & WordsReaderOptions.WordIsLessOrEquals400Chars) == WordsReaderOptions.WordIsLessOrEquals400Chars)
                    {
                        Assert.That(words.Values.Sum(), Is.EqualTo(105));
                    }
                    else if (options == WordsReaderOptions.WordHasAtLeast2Vowels)
                    {
                        Assert.That(words.Values.Sum(), Is.EqualTo(106));
                    }
                    else if (options == WordsReaderOptions.WordIsLessOrEquals400Chars)
                    {
                        Assert.That(words.Values.Sum(), Is.EqualTo(107));
                    }
                }
            }
        }

        //[TestCase("hostname1parameter")]
        //[TestCase("hostname2parameter")]
        //public void Example_TestHostName(string hostname)
        //{
           
        //}
    }
}