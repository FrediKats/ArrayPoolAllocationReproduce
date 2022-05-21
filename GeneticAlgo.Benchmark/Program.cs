// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Entities;

Summary? summary = BenchmarkRunner.Run<FileSystemBenchmark>();
Console.Read();

[SimpleJob]
[MemoryDiagnoser]
public class FileSystemBenchmark
{
    [Params(true, false)]
    public bool UseShared { get; set; }

    [Benchmark]
    public void Read()
    {
        AllocationMode.UseShared = UseShared;

        const int iterationCount = 10;
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