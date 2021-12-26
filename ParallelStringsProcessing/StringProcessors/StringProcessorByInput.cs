using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorByInput : IAsyncStringProcessor
    {
        private readonly ConcurrentDictionary<char, int> _counts = new ConcurrentDictionary<char, int>();
        private readonly string _str;
        public string Input => _str;
        private readonly object _lockObject = new Object();

        public StringProcessorByInput(string str)
        {
            _str = str;
        }

        public async Task<IDictionary<char, int>> ProcessAsync()
        {
            var taskFactory = new TaskFactory();
            var tasks = new List<Task>();
            foreach (var symbol in _str)
            {
                tasks.Add(taskFactory.StartNew(() => ProcessSymbol(symbol)));
            }

            await Task.WhenAll(tasks);

            return _counts;
        }

        private void ProcessSymbol(char symbol)
        {
            lock (_lockObject)
            {
                if (_counts.ContainsKey(symbol))
                {
                    _counts[symbol]++;
                }
                else
                {
                    if (!_counts.TryAdd(symbol, 0))
                    {
                        throw new Exception("Failed to add key to the dictionary");
                    }
                }
            }
        }
    }
}