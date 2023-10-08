using System;
using System.Collections.Generic;
using System.Linq;

class LivingOrganism
{
    public int Energy { get; set; }
    public int Age { get; set; }
    public double Size { get; set; }

    public LivingOrganism(int energy, int age, double size)
    {
        Energy = energy;
        Age = age;
        Size = size;
    }

    public virtual void Grow()
    {
        Size += 1;
    }
}

class Animal : LivingOrganism
{
    public string Species { get; set; }

    public Animal(int energy, int age, double size, string species)
        : base(energy, age, size)
    {
        Species = species;
    }
}

class Plant : LivingOrganism
{
    public string Type { get; set; }

    public Plant(int energy, int age, double size, string type)
        : base(energy, age, size)
    {
        Type = type;
    }
}

class Microorganism : LivingOrganism
{
    public string Classification { get; set; }

    public Microorganism(int energy, int age, double size, string classification)
        : base(energy, age, size)
    {
        Classification = classification;
    }
}

interface IReproducible
{
    LivingOrganism Reproduce();
}

interface IPredator
{
    void Hunt(LivingOrganism prey);
}

class Ecosystem
{
    private List<LivingOrganism> organisms;

    public Ecosystem(List<LivingOrganism> initialOrganisms)
    {
        organisms = initialOrganisms;
    }

    public void Simulate(int numSteps)
    {
        for (int step = 0; step < numSteps; step++)
        {
            foreach (var organism in organisms.ToList())
            {
                if (organism is IReproducible && new Random().NextDouble() < 0.2)
                {
                    var newOrganism = (organism as IReproducible).Reproduce();
                    organisms.Add(newOrganism);
                }

                if (organism is IPredator)
                {
                    var potentialPrey = organisms.Where(o => o != organism).ToList();
                    if (potentialPrey.Any())
                    {
                        var prey = potentialPrey[new Random().Next(potentialPrey.Count)];
                        (organism as IPredator).Hunt(prey);
                    }
                }

                organism.Grow();
                organism.Energy -= 1;
                organism.Age += 1;

                if (organism.Energy <= 0 || organism.Age >= 10)
                {
                    organisms.Remove(organism);
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var lion = new Animal(70, 6, 3, "Lion");
        var gazelle = new Animal(50, 5, 2.5, "Gazelle");
        var oakTree = new Plant(20, 20, 6, "Oak");
        var bacteria = new Microorganism(5, 1, 0.01, "Bacteria");

        var ecosystem = new Ecosystem(new List<LivingOrganism> { lion, gazelle, oakTree, bacteria });

        ecosystem.Simulate(100);
    }
}