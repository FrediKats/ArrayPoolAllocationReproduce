using System.Numerics;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;

namespace GeneticAlgo.Shared;

public class Settings
{
    public static readonly Vector2 Goal = new((float) 1.0, (float) 1.0);
    public static readonly double LimitSpeed = 0.03;
    public static readonly int StepsCount = 400;
    public static readonly BarrierCircle[] Barriers;

    static Settings()
    {
        var circleGenerator = new CircleGenerator(1, 3, Goal);
        Barriers = circleGenerator.GetCircles();
    }
}