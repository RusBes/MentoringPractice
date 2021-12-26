using System;
using System.Collections.Generic;
using System.Linq;

namespace ParallelStringsProcessing
{
    public class Helper
    {
        public static void DisplayResult(IDictionary<char, int> charsHistogram)
        {
            foreach (var kvp in charsHistogram.OrderBy(x => x.Key))
            {
                Console.WriteLine($"{kvp.Key}:\t{kvp.Value}");
            }

            if (charsHistogram.Count == 0)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}