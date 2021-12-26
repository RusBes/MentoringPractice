using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorSequential : IAsyncStringProcessor
    {
        private readonly Dictionary<char, int> _counts = new Dictionary<char, int>();
        private readonly string _str;
        public string Input => _str;

        public StringProcessorSequential(string str)
        {
            _str = str;
        }

        public Dictionary<char, int> Process()
        {
            foreach (var symbol in RandomStringGenerator.AllowedSymbols)
            {
                ProcessSymbol(symbol);
            }

            return _counts;
        }

        private void ProcessSymbol(char symbol)
        {
            _counts.TryAdd(symbol, 0);

            for (int i = 0; i < _str.Length; i++)
            {
                if(_str[i] == symbol)
                {
                    _counts[symbol]++;
                }
            }
        }

        public Task<IDictionary<char, int>> ProcessAsync()
        {
            return Task.FromResult(Process() as IDictionary<char, int>);
        }
    }
}
