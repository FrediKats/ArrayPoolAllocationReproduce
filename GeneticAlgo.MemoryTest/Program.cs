using GeneticAlgo.Shared.Entities;

namespace GeneticAlgo.MemoryTest;

public class Program
{
    public static void Main(string[] args)
    {
        var execution = new Execution(4, 4, 1000, 500);
        while (true)
        {
            execution.ExecuteIteration();
            if (execution.Population.AllDead())
            {
                execution.ExecuteIfDead();
            }
        }
    }
}
