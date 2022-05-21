using System.Numerics;

namespace GeneticAlgo.Shared.Entities;

public class Brain
{
    private const double MutateChance = 0.025;

    public Vector2[] Directions;
    
    public int Step { get; set; }
    public int GenCount { get; }

    public Brain(int size)
    {
        Directions = AllocationMode.GetPool().Rent(size);
        Step = 0;
        GenCount = size;

        Randomize();
    }

    public void Randomize()
    {
        for (var i = 0; i < GenCount; i++)
        {
            float angle = Random.Shared.NextSingle() * 2 * MathF.PI;
            float x = MathF.Cos(angle) * 0.005f;
            float y = MathF.Sin(angle) * 0.005f;
            Directions[i] = new Vector2(x, y);
        }
    }
    public Brain CloneBrain()
    {
        var clone = new Brain(GenCount);
        for (var i = 0; i < GenCount; i++)
            clone.Directions[i] = Directions[i];
        return clone;
    }

    public void Mutate()
    {
        for (var i = 0; i < GenCount; i++)
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
        AllocationMode.GetPool().Return(Directions);
    }
}