using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using ParallelStringsProcessing.StringProcessors;

namespace ParallelStringsProcessing
{
    class Program
    {
        private const string SampleInput1000 = "iOPefcegqa2Pytpd8FQOgAI947rLiNLbRedFRkLSymlwMVRE4V2dTvyifPHHpY6DuZ6UQKrty6uyTqsgMnOSRHn1kz9bDECAOibrX3Trdhjua9UU3azCjkqNLCJVkQQu1ukkoBopZ3oj5YzLFkFcJAVDwtGVS5gsWUyLUjF0RKhN2HpSH6pAknL18HT0zy42V2si8oW1FjQ8XIHPdTnUNGhii6lNVKaxmlXAtHTxHU0a4ldW9dWgkIOoTj9acJF0JxrlpH139VNYLHL3zuNrsZmE4IljHSu6oAwEZXGxIZgu0VdM7cGN1sHPLNOH0RumAPJkPgcCOujySXJHe9LElhC2YhYli85WeZUklx3wAd7GJN3sdHc0fQaxmNrhnWyPwstxD70MDTEhJ2SkkFhB2CQtY266fUiiKoOVyEpFlvXrMYoq0H4iOXsucA9gA4GOk05Zm3qguCufK6HAmMc7wAZr1ysyEieopJgq0Ud7VVsMcNkEx8yTZHWKak7TiXCl4fVawVexiEIbMNonOoaFoquIS3QPVPu6THo3XjmCZBKu7U5S1QFZW33LQmnzFUCYoKNe6SvEC7kng9J575tFK1gWkdLK2r4F8RGtgYUizBXxRneqDp2Ha3OsDWZzgLhPBfOUYuWfikkJDOF37zglvuECDSQp7ZCOYhWL8PXnyV7VReptR42lE2ylbMRGEGgOi2p0Xy4OcYSAvOp6c36b6VK3OKjS6qJSopmVUU3XAmiLJyus2TDhhFQSRTMmPKpqo9QGBzJn9GBQM7suUvvKNbKj8ZdBhohV3X2JIoRBKuhbd9fSAwXyTfDmuggHvhfp6LTUcJMfeUAxZIf9tqaQwObWtOtBhKkCPwDXlQpvKRt7fksMBHsFHuYoj4aTuL5zj6ICenTDBjfFZDqi1hAaWfzz7tnOP2B8tZtHCqHssPd4osUGLXxmk4DEDndQ9eseIgqAOJGjammHb4kk8HDXdtTwWpETmvsO27UhCUwC";
        
        static void Main(string[] args)
        {
            var strs = new[]
            {
                RandomStringGenerator.GenerateRandomString(500),
                RandomStringGenerator.GenerateRandomString(1000),
                RandomStringGenerator.GenerateRandomString(100000)
            };
            // var str = RandomStringGenerator.GenerateRandomString(1000);
            // var str = SampleInput1000;

            foreach (var str in strs)
            {
                Console.WriteLine($"Processing string of length {str.Length}");
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByDictionary(str), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByDictionary(str));
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByDictionaryWithParallelForEach(str), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByDictionaryWithParallelForEach(str));
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 256), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 256));
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 32), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 32));
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 8), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 8));
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 2), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 2));
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 1), true);
                ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByStringParts(str, 1));
                // ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByDictionaryWithInterlocked(str)); // doesn't work
                // ProcessAndDisplayElapsedTimeAndValidate(new StringProcessorByInput(str)); // doesn't work
            
                // Helper.DisplayResult(result);
                Console.WriteLine();
                ProcessAndDisplayElapsedTime(new StringProcessorSequential(str), true);
                ProcessAndDisplayElapsedTime(new StringProcessorSequential(str));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static IDictionary<char, int> ProcessAndDisplayElapsedTimeAndValidate(IAsyncStringProcessor processor, bool warmup = false)
        {
            var actual = ProcessAndDisplayElapsedTime(processor, warmup);
            
            if (!warmup)
            {
                var expected = new StringProcessorSequential(processor.Input).Process();
                Validate(expected, actual);
            }

            return actual;
        }
        
        private static IDictionary<char, int> ProcessAndDisplayElapsedTime(IAsyncStringProcessor processor, bool warmup = false)
        {
            // var temp = processor.ProcessAsync().Result; // errors if uncomment
            
            var sw = Stopwatch.StartNew();
            var result = processor.ProcessAsync().Result;
            var elapsed = sw.ElapsedTicks;
            if (!warmup)
            {
                Console.WriteLine($"{processor.GetType().Name} elapsed milliseconds: {elapsed}");
            }

            return result;
        }
        
        private static void Validate(IDictionary<char, int> expected, IDictionary<char, int> actual)
        {
            var success = true;
            foreach (var kvp in expected)
            {
                if (actual.ContainsKey(kvp.Key) && actual[kvp.Key] == kvp.Value)
                {
                    continue;
                }

                success = false;
                Console.WriteLine($"Validation failed. Expected char '{kvp.Key}' to occur {kvp.Value} times, but was {actual[kvp.Key]}");
            }

            if (success)
            {
                Console.WriteLine("Validation successful");
            }
        }
    }
}
