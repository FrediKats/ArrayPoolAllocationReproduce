﻿using System.Buffers;
using System.Numerics;

namespace GeneticAlgo.Shared.Entities;

public static class Counter
{
    public static int MaxValue { get; set; }
    public static int Value { get; set; }
    public static void Change(int value)
    {
        Value += value;
        if (Value > MaxValue)
        {
            MaxValue = Value;
            Console.WriteLine(MaxValue);
        }
    }
}

public struct Brain
{
    //public static ArrayPool<Vector2> MainPool = ArrayPool<Vector2>.Create(Settings.StepsCount, 4000);
    public static ArrayPool<Vector2> MainPool => ArrayPool<Vector2>.Shared;

    public Vector2[] Directions;
    public int Step;
    public double MutateChance;

    public Brain(int size)
    {
        Directions = MainPool.Rent(size);
        Counter.Change(1);
        Step = 0;
        MutateChance = 0.025;
        Randomize();
    }

    public void Randomize()
    {
        for (var i = 0; i < Directions.Length; i++)
        {
            var angle = Random.Shared.NextDouble() * 2 * Math.PI;
            var x = Math.Cos(angle) * 0.005;
            var y = Math.Sin(angle) * 0.005;
            Directions[i] = new Vector2((float) x, (float) y);
        }
    }
    public Brain CloneBrain()
    {
        Brain clone = new Brain(Directions.Length);
        for (int i = 0; i < Directions.Length; i++)
            clone.Directions[i] = Directions[i];
        return clone;
    }
    public void Mutate()
    {
        for (var i = 0; i < Directions.Length; i++)
        {
            var rand = Random.Shared.NextDouble();
            if (rand > MutateChance) continue;
            var angle = Random.Shared.NextDouble() * 2 * Math.PI;
            var x = Math.Cos(angle) * 0.005;
            var y = Math.Sin(angle) * 0.005;
            Directions[i] = new Vector2((float) x, (float) y);
        }
    }

    public void Clear()
    {
        MainPool.Return(Directions);
        Counter.Change(-1);
    }
}