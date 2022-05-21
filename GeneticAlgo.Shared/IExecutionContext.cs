using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared
{
    public interface IExecutionContext
    {
        BarrierCircle[] GetCircles();
    }
}