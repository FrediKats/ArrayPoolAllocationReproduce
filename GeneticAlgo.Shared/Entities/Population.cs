namespace GeneticAlgo.Shared.Entities;

public class Population
{
    public Dot[] Dots;

    private int _minStep;

    public Population(int minStep, int dotsCount)
    {
        _minStep = minStep;
        Dots = new Dot[dotsCount];

        for (int i = 0; i < Dots.Length; i++)
            Dots[i] = new Dot();
    }

    public void NextIteration(double width, double height)
    {
        for (int i = 0; i < Dots.Length; i++)
        {
            if (Dots[i].Brain.Step > _minStep + 200)
            {
                Dots[i].IsDead = true;
                Dots[i].IsSlow = true;
            }
            else
            {
                Dots[i].NextIteration(width, height);
            }
        }
    }

    public bool AllDead()
    {
        for (int i = 0; i < Dots.Length; i++) {
            if (!Dots[i].IsDead && !Dots[i].IsReached) { 
                return false;
            }
        }

        return true;
    }
    
    public void NextGeneration() 
    {
        var newDots = new Dot[Dots.Length];
        Dot bestDot = SetBestDot();

        newDots[0] = bestDot.GetBaby();
        double fitnessSum = Dots.Sum(t => t.GetFitness());
        for (int i = 1; i < newDots.Length; i++)
        {
            Dot parent = SelectParent(fitnessSum);
            Dot baby = parent.GetBaby();
            newDots[i] = baby;
        }
        
        for (int i = 0; i < newDots.Length; i++)
        {
            Dots[i].Clear();
        }
        Dots = newDots;
    }

    public void MutateBabies() 
    {
        for (int i = 1; i< Dots.Length; i++)
            Dots[i].Brain.Mutate();
    }

    private Dot SelectParent(double fitnessSum)
    {
        double rand = Random.Shared.NextDouble() * fitnessSum;
        double runningSum = 0;

        foreach (Dot selectedDot in Dots)
        {
            runningSum += selectedDot.GetFitness();
            if (runningSum > rand)
                return selectedDot;
        }

        throw new Exception("Unreached code");
    }

    private Dot SetBestDot() 
    {
        Dot bestDot = Dots.MaxBy(d => d.GetFitness()) ?? throw new ArgumentException(nameof(Dots));

        if (bestDot.IsReached)
            _minStep = bestDot.Brain.Step;

        return bestDot;
    }
}