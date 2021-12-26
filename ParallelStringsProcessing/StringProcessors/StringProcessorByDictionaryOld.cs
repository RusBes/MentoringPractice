using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public class StringProcessorByDictionaryOld
    {
        private readonly ConcurrentDictionary<char, int> _counts = new ConcurrentDictionary<char, int>();
        private readonly string _str;

        public StringProcessorByDictionaryOld(string str)
        {
            _str = str;
        }

        public async Task Process()
        {
            var taskFactory = new TaskFactory();
            var tasks = new List<Task>();
            foreach (var symbol in RandomStringGenerator.AllowedSymbols)
            {
                tasks.Add(taskFactory.StartNew(() => ProcessSymbol(symbol)));
            }

            await Task.WhenAll(tasks);


            DisplayResult();
        }

        private void ProcessSymbol(char symbol)
        {
            for (int i = 0; i < _str.Length; i++)
            {
                if (_str[i] == symbol)
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

        private void DisplayResult()
        {
            foreach (var kvp in _counts)
            {
                Console.WriteLine($"{kvp.Key}:\t{kvp.Value}");
            }

            if (_counts.Count == 0)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}