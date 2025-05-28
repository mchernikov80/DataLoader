using PromIt.DataLoader.Console.Infrastructure.Readers;

namespace PromIt.DataLoader.Console.Infrastructure.Loaders
{
    /// <summary>
    /// Data Loader.
    /// </summary>
    public class DataDbLoader
    {
        private readonly WordsReader reader;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataDbLoader(WordsReader reader)
        { 
            this.reader = reader;
        }

        public async Task LoadAsync(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    
                    var loadData = await reader.ReadAsync(stream);
                    

                    //foreach (var data in loadData) 
                    //{
                    //    if (data.Amount > 3)
                    //    {
                    //        // load to DB
                    //    }
                    //}

                }
            }
        }
    }
}
