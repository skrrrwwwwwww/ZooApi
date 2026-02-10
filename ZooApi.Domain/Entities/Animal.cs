namespace ZooApi.Domain.Entities;

public class Animal(string name, string species)
{
    public Guid Id { get; init; }
    
    public string Name { get; private set; } = !string.IsNullOrWhiteSpace(name) 
        ? name 
        : throw new ArgumentException("Имя не может быть пустым", nameof(name)); 

    public string Species { get; private set; } = !string.IsNullOrWhiteSpace(species) 
        ? species 
        : throw new ArgumentException("Вид не может быть пустым", nameof(species));

    public int Energy { get; private set; } = 100;
    
    public int Intensity { get; private set; }

    
    protected Animal() : this("Internal", "Internal") { }

    public void Feed(int amount) => Energy = amount is < 1 or > 100 
        ? throw new ArgumentOutOfRangeException(nameof(amount), "Еда должна быть в диапазоне 1-100") 
        : Math.Min(100, Energy + amount);

    public void Play(int intensity)
    {
        int cost = intensity > 0 
            ? intensity * 2 
            : throw new ArgumentException("Интенсивность должна быть больше 0", nameof(intensity));

        if (Energy < cost)
            throw new InvalidOperationException("Животное слишком устало для такой активной игры");

        Energy -= cost;
        
        Intensity = intensity;
    }
}