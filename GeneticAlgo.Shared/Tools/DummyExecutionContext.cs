using System.Numerics;
using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class DummyExecutionContext : IExecutionContext
{
    private readonly int _circleCount;
    private readonly int _maximumValue;
    private readonly Vector2 _goal;
    private readonly BarrierCircle[] _circles;

    public DummyExecutionContext(int maximumValue, int circleCount)
    {
        _maximumValue = maximumValue;
        _circleCount = circleCount;
        _goal = new Vector2((float) 1.0, (float) 1.0);
        _circles = ReportCircles();
    }

    private double NextPosition => Random.Shared.NextDouble() * _maximumValue;
    private double NextRadius => 0.3 + Random.Shared.NextDouble() * _maximumValue * 2;
    
    public BarrierCircle[] GetCircles()
    {
        return _circles;
    }

    public BarrierCircle[] ReportCircles()
    {
        var circles = new BarrierCircle[_circleCount];
        for (int i = 0; i < _circleCount; i++)
        {
            var positionX = NextPosition;
            var positionY = NextPosition;
            var radius = NextRadius;
            while (Math.Sqrt(positionX * positionX + positionY * positionY) < radius || 
                   Math.Sqrt((positionX - _goal.X) * (positionX - _goal.X) + 
                             (positionY - _goal.Y) * (positionY - _goal.Y)) < radius)
            {
                positionX = NextPosition;
                positionY = NextPosition;
                radius = NextRadius;
            }

            circles[i] = new BarrierCircle(new Point(positionX, positionY), 50 * radius);
        }

        return circles;
    }
}