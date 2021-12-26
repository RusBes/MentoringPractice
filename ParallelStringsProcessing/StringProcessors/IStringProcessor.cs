using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelStringsProcessing.StringProcessors
{
    public interface IAsyncStringProcessor
    {
        string Input { get; }
        
        Task<IDictionary<char, int>> ProcessAsync();
    }
}