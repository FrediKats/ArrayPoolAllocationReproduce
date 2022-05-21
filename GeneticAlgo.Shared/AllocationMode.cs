using System.Buffers;
using System.Numerics;

namespace GeneticAlgo.Shared;

public class AllocationMode
{
    public static bool UseShared = true;

    private static readonly ArrayPool<Vector2> CreatedPool = ArrayPool<Vector2>.Create(Settings.StepsCount, 4000);

    public static ArrayPool<Vector2> GetPool()
    {
        if (UseShared)
            return ArrayPool<Vector2>.Shared;

        return CreatedPool;
    }
}