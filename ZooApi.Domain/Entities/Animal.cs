namespace ZooApi.Domain.Entities;

public class Animal(string name, string species)
{
    public int Id { get; init; }
    public string Name { get; private set; } = !string.IsNullOrWhiteSpace(name) 
                                                ? name 
                                                : throw new ArgumentException("Имя пустое");
    public string Species { get; private set; } = !string.IsNullOrWhiteSpace(species) 
                                                ? species 
                                                : throw new ArgumentException("Вид пустой");
    public int Energy { get; private set; } = 100;
    
    protected Animal () : this("Internal", "Internal") { }

    public void Feed(int amount) => Energy = amount is < 1 or > 100 
        ? throw new ArgumentOutOfRangeException("Еда должна быть 1-100",  nameof(amount)) 
        : Math.Min(100, Energy + amount);

    public void Play(int intensity)
    {
        int cost = intensity > 0 ? intensity * 2 : throw new ArgumentException("Интенсивность должна быть больше 0", nameof(intensity));

        Energy = Energy >= cost ? Energy - cost : throw new InvalidOperationException("Животное слишком устало для такой активной игры");
    }
}