using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ParallelStringsProcessing.Models;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorByDictionaryWithInterlocked : IAsyncStringProcessor
    {
        private readonly ConcurrentDictionary<char, IntWrapper> _counts = new ConcurrentDictionary<char, IntWrapper>();
        private readonly string _str;
        public string Input => _str;
        
        public StringProcessorByDictionaryWithInterlocked(string str)
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

            return _counts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
        }

        private void ProcessSymbol(char symbol)
        {
            _counts.TryAdd(symbol, new IntWrapper {Value = 0});

            for (int i = 0; i < _str.Length; i++)
            {
                if(_str[i] == symbol)
                {
                    var sumbolCounter = _counts[symbol].Value;
                    Interlocked.Increment(ref sumbolCounter);
                }
            }
        }
    }
}
