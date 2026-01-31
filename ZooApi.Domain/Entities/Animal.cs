namespace ZooApi.Domain.Entities;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string Species { get; private set; } = string.Empty;
    public int Energy { get; private set; }
    
    protected Animal () { }

    public Animal(string name, string species)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым", nameof(name));
                
        if (string.IsNullOrWhiteSpace(species))
            throw new ArgumentException("Вид не может быть пустым", nameof(species));
        
        Name = name;
        Species = species;
        Energy = 100;
    }

    public void Feed(int amount)
    {
        if (amount is < 1 or > 100) 
            throw new ArgumentOutOfRangeException("Еда должна быть 1-100",  nameof(amount));
        Energy = Math.Min(100, Energy + amount);
    }

    public void Play(int intensity)
    {
        if (intensity < 1)
            throw new ArgumentException("Интенсивность должна быть больше 0", nameof(intensity));

        int energyCost = intensity * 2;

        if (Energy < energyCost) 
            throw new InvalidOperationException("Животное слишком устало для такой активной игры");

        Energy -= energyCost;
    }
}