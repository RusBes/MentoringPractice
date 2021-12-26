using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorByDictionary : IAsyncStringProcessor
    {
        private readonly ConcurrentDictionary<char, int> _counts = new ConcurrentDictionary<char, int>();
        private readonly string _str;
        public string Input => _str;

        public StringProcessorByDictionary(string str)
        {
            _str = str;
        }

        public async Task<IDictionary<char, int>> ProcessAsync()
        {
            var taskFactory = new TaskFactory();
            var tasks = new List<Task>();
            foreach (var symbol in RandomStringGenerator.AllowedSymbols)
            {
                tasks.Add(taskFactory.StartNew(() => ProcessSymbol(symbol)));
            }

            await Task.WhenAll(tasks);

            return _counts;
        }

        private void ProcessSymbol(char symbol)
        {
            _counts.TryAdd(symbol, 0);

            // Console.WriteLine($"Processing symbol '{symbol}'");
            for (int i = 0; i < _str.Length; i++)
            {
                if (_str[i] == symbol)
                {
                    _counts[symbol]++;
                }
            }
        }
    }
}