using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorByStringParts : IAsyncStringProcessor
    {
        private readonly ConcurrentDictionary<char, int> _counts = new ConcurrentDictionary<char, int>();
        private readonly string _str;
        public string Input => _str;
        private readonly int _partsCount;

        public StringProcessorByStringParts(string str, int partsCount)
        {
            _str = str;
            _partsCount = partsCount;
        }

        public async Task<IDictionary<char, int>> ProcessAsync()
        {
            var taskFactory = new TaskFactory();
            var tasks = new List<Task<Dictionary<char, int>>>();

            // split into substrings
            var subStrs = new List<string>();
            var subStrLength = _str.Length / _partsCount;
            for (int i = 0; i < _partsCount; i++)
            {
                var startIndex = i * subStrLength;
                var subStr = i == _partsCount - 1
                    ? _str.Substring(startIndex)
                    : _str.Substring(startIndex, subStrLength);

                subStrs.Add(subStr);
            }

            // process substrings
            foreach (var subStr in subStrs)
            {
                // use one of existing methods to process substrings. For example sequential method 
                tasks.Add(taskFactory.StartNew(() => new StringProcessorSequential(subStr).Process())); 
            }

            var subResults = await Task.WhenAll(tasks);

            // merge sub results
            foreach (var subResult in subResults)
            {
                foreach (var kvp in subResult)
                {
                    var symbol = kvp.Key;
                    if (_counts.ContainsKey(symbol))
                    {
                        _counts[symbol] += kvp.Value;
                    }
                    else
                    {
                        if (!_counts.TryAdd(symbol, kvp.Value))
                        {
                            throw new Exception("Failed to add key to the dictionary");
                        }
                    }
                }
            }

            return _counts;
        }

        // private void ProcessSymbol(char symbol)
        // {
        //     lock (_lockObject)
        //     {
        //         if (_counts.ContainsKey(symbol))
        //         {
        //             _counts[symbol]++;
        //         }
        //         else
        //         {
        //             if (!_counts.TryAdd(symbol, 0))
        //             {
        //                 throw new Exception("Failed to add key to the dictionary");
        //             }
        //         }
        //     }
        // }
    }
}