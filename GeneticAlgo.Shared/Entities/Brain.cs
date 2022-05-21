using System.Buffers;
using System.Numerics;

namespace GeneticAlgo.Shared.Entities;

public class Brain
{
    //public static ArrayPool<Vector2> MainPool = ArrayPool<Vector2>.Create(Settings.StepsCount, 4000);
    public static ArrayPool<Vector2> MainPool => ArrayPool<Vector2>.Shared;

    public Vector2[] Directions;
    public int Step;
    public double MutateChance;

    public Brain(int size)
    {
        Directions = MainPool.Rent(size);
        Step = 0;
        MutateChance = 0.025;
        Randomize();
    }

    public void Randomize()
    {
        for (var i = 0; i < Directions.Length; i++)
        {
            float angle = Random.Shared.NextSingle() * 2 * MathF.PI;
            float x = MathF.Cos(angle) * 0.005f;
            float y = MathF.Sin(angle) * 0.005f;
            Directions[i] = new Vector2(x, y);
        }
    }
    public Brain CloneBrain()
    {
        var clone = new Brain(Directions.Length);
        for (var i = 0; i < Directions.Length; i++)
            clone.Directions[i] = Directions[i];
        return clone;
    }

    public void Mutate()
    {
        for (var i = 0; i < Directions.Length; i++)
        {
            double rand = Random.Shared.NextDouble();
            if (rand > MutateChance) continue;

            float angle = Random.Shared.NextSingle() * 2 * MathF.PI;
            float x = MathF.Cos(angle) * 0.005f;
            float y = MathF.Sin(angle) * 0.005f;
            Directions[i] = new Vector2(x, y);
        }
    }

    public void Clear()
    {
        MainPool.Return(Directions);
    }
}