namespace ZooApi.Domain.Entities;

public class Animal
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Species { get; private set; } = string.Empty;
    public int Energy { get; private set; }
    
    private Animal() { } // EF Core
    
    public Animal(string name, string species)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Species = species ?? throw new ArgumentNullException(nameof(species));
        Energy = 100;
    }
    
    public void Feed(int amount)
    {
        if (amount < 1 || amount > 100)
            throw new ArgumentException("Еда должна быть 1-100");
        Energy = Math.Min(100, Energy + amount);
    }
}