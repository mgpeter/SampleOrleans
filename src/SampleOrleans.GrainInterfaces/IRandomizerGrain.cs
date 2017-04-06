using System.Threading.Tasks;
using Orleans;

namespace SampleOrleans.GrainInterfaces
{
    /// <summary>
    /// Grain interface IGrain1
    /// </summary>
    public interface IRandomizerGrain : IGrainWithIntegerKey
    {
        Task<string> SayRandomWelcome(string name);
    }
}
