using System.Numerics;

namespace GeneticAlgo.Shared.Entities;

public class Dot
{
    public Vector2 Position;
    public Vector2 Speed;
    public Vector2 Acceleration;

    public Brain Brain;
    public bool IsDead;
    public bool IsReached;
    public bool IsSlow;

    public Dot() : this(new Brain(Settings.StepsCount))
    {
        
    }

    public Dot(Brain other)
    {
        Brain = other;
        IsDead = false;
        IsReached = false;
        IsSlow = false;
        Position = new Vector2(0.0f, 0.0f);
        Speed = new Vector2(0.0f, 0.0f);
        Acceleration = new Vector2(0.0f, 0.0f);
    }

    public void Move()
    {
        if (Brain.Step < Brain.Directions.Length)
        {
            Acceleration = Brain.Directions[Brain.Step];
            Brain.Step++;
        }
        else
        {
            IsDead = true;
            IsSlow = true;
        }
        
        var newVec = new Vector2(Speed.X + Acceleration.X, Speed.Y + Acceleration.Y);
        if (newVec.Length() >= Settings.LimitSpeed)
        {
            Speed.X %= (float) Settings.LimitSpeed;
            Speed.Y %= (float) Settings.LimitSpeed;
            
        }
        
        Speed.X += Acceleration.X;
        Speed.Y += Acceleration.Y;
        Position.X += Speed.X;
        Position.Y += Speed.Y;
    }

    public void NextIteration(double width, double height)
    {
        if (!IsDead && !IsReached)
        {
            Move();
            double distX = Position.X - Settings.Goal.X;
            double distY = Position.Y - Settings.Goal.Y;
            double dist = Math.Sqrt(distX * distX + distY * distY);
            if (Position.X > width / 2 || Position.Y > height / 2 || Position.X < -width / 2 ||
                Position.Y < -height / 2 || IsBumped())
                IsDead = true;
            else if (dist < 0.025)
                IsReached = true;
        }
    }
    
    public Dot GetBaby()
    {
        return new Dot(Brain.CloneBrain());
    }
    public bool IsBumped()
    {
        var barriers = Settings.Barriers;
        foreach (var barrier in barriers)
        {
            var radius = barrier.Radius / 155;
            var distX = barrier.Center.X - Position.X;
            var distY = barrier.Center.Y - Position.Y;
            if (Math.Sqrt(distX * distX + distY * distY) > radius) continue;
            return true;
        }

        return false;
    }
    
    public double GetFitness()
    {
        if (IsReached)
            return 50000.0 / (Brain.Step * Brain.Step);

        if (!IsSlow)
        {
            double distX = Position.X - Settings.Goal.X;
            double distY = Position.Y - Settings.Goal.Y;
            double dist = Math.Sqrt(distX * distX + distY * distY);
            return 0.1 / (dist * dist);
        }

        return 0.0;
    }

    public void Clear()
    {
        Brain.Clear();
    }
}