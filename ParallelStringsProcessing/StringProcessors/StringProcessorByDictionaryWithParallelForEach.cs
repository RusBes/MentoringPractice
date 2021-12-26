using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorByDictionaryWithParallelForEach : IAsyncStringProcessor
    {
        private readonly ConcurrentDictionary<char, int> _counts = new ConcurrentDictionary<char, int>();
        private readonly string _str;
        public string Input => _str;
        
        public StringProcessorByDictionaryWithParallelForEach(string str)
        {
            _str = str;
        }

        public Task<IDictionary<char, int>> ProcessAsync()
        {
            Parallel.ForEach(RandomStringGenerator.AllowedSymbols, symbol => ProcessSymbol(symbol));

            return Task.FromResult(_counts as IDictionary<char, int>);
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
    }
}
