using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelStringsProcessing
{
    public class StringProcessor
    {
        private Dictionary<char, int> _counts = new Dictionary<char, int>();
        private string _str;

        public StringProcessor(string str)
        {
            _str = str;
        }

        public async Task Process()
        {
            var taskFactory = new TaskFactory();
            var tasks = new List<Task>();
            foreach (var symbol in RandomStringGenerator.AllowedSymbols)
            {
                tasks.Add(new TaskFactory().StartNew(() => ProcessSymbol(symbol)));
                //ProcessSymbol(symbol);
            }
            await Task.WhenAll(tasks);



            DisplayResult();
        }

        private void ProcessSymbol(char symbol)
        {
            Console.WriteLine($"Processing symbol '{symbol}'");
            for (int i = 0; i < _str.Length; i++)
            {
                if(_str[i] == symbol)
                {
                    if (_counts.ContainsKey(symbol))
                    {
                        Console.WriteLine($"There's already symbol '{symbol}'");
                        _counts[symbol]++;
                    }
                    else
                    {
                        _counts.Add(symbol, 0);
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

            if(_counts.Count == 0)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}
