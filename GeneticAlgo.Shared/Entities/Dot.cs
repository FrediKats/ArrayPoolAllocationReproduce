using System.Numerics;

namespace GeneticAlgo.Shared.Entities;

public class Dot
{
    private Vector2 _position;
    private Vector2 _speed;
    private Vector2 _acceleration;

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
        _position = new Vector2(0.0f, 0.0f);
        _speed = new Vector2(0.0f, 0.0f);
        _acceleration = new Vector2(0.0f, 0.0f);
    }

    private void Move()
    {
        if (Brain.Step < Brain.Directions.Length)
        {
            _acceleration = Brain.Directions[Brain.Step];
            Brain.Step++;
        }
        else
        {
            IsDead = true;
            IsSlow = true;
        }
        
        var newVec = new Vector2(_speed.X + _acceleration.X, _speed.Y + _acceleration.Y);
        if (newVec.Length() >= Settings.LimitSpeed)
        {
            _speed.X %= (float) Settings.LimitSpeed;
            _speed.Y %= (float) Settings.LimitSpeed;
            
        }
        
        _speed.X += _acceleration.X;
        _speed.Y += _acceleration.Y;
        _position.X += _speed.X;
        _position.Y += _speed.Y;
    }

    public void NextIteration(double width, double height)
    {
        if (IsDead || IsReached)
            return;
        
        Move();
        double distX = _position.X - Settings.Goal.X;
        double distY = _position.Y - Settings.Goal.Y;
        double dist = Math.Sqrt(distX * distX + distY * distY);

        if (_position.X > width / 2
            || _position.Y > height / 2
            || _position.X < -width / 2
            || _position.Y < -height / 2
            || IsBumped())
        {
            IsDead = true;
        }
        else if (dist < 0.025)
        {
            IsReached = true;
        }
    }
    
    public Dot GetBaby()
    {
        return new Dot(Brain.CloneBrain());
    }

    private bool IsBumped()
    {
        var barriers = Settings.Barriers;
        foreach (var barrier in barriers)
        {
            var radius = barrier.Radius / 155;
            var distX = barrier.Center.X - _position.X;
            var distY = barrier.Center.Y - _position.Y;
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
            double distX = _position.X - Settings.Goal.X;
            double distY = _position.Y - Settings.Goal.Y;
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