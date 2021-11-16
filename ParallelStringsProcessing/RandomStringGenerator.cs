using System;

namespace ParallelStringsProcessing
{
    public static class RandomStringGenerator
    {
        public static string AllowedSymbols { get; set; } = GetAllowedSymbols();
        private static Random _rnd = new Random();

        public static string GenerateRandomString(int count)
        {
            var res = "";
            var maxValue = AllowedSymbols.Length;
            for (int i = 0; i < count; i++)
            {
                res += AllowedSymbols[_rnd.Next(maxValue)];
            }

            return res;
        }

        private static string GetAllowedSymbols()
        {
            var res = "";
            for (int i = 0; i < 26; i++)
            {
                res += Convert.ToChar(i + 65);
                res += Convert.ToChar(i + 97);
            }

            for (int i = 0; i < 10; i++)
            {
                res += Convert.ToChar(i + 48);
            }

            return res;
        }
    }
}
