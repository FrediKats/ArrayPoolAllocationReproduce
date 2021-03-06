using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Entities;

namespace GeneticAlgo.MemoryTest;

public class Program
{
    public static void Main(string[] args)
    {
        Execute();
        AllocationMode.UseShared = false;
        Execute();
    }

    public static void Execute()
    {
        const int iterationCount = 500;
        var execution = new Execution(4, 4, 1000, 500);

        int restartCount = iterationCount;
        while (restartCount > 0)
        {
            execution.ExecuteIteration();
            if (execution.Population.AllDead())
            {
                execution.ExecuteIfDead();
                restartCount--;
            }
        }
    }
}
