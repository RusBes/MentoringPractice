namespace ParallelStringsProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = RandomStringGenerator.GenerateRandomString(1000);
            new StringProcessor(str).Process().Wait();
        }
    }
}
