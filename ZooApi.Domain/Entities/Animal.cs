namespace ZooApi.Domain.Entities;

public class Animal(string name, string species, Guid ownerId)
{
    // Стоимость игры = интенсивность * 2, а максимальная энергия — 100.
    // Граница держит cost в пределах Energy и исключает переполнение int.
    public const int MaxIntensity = 50;

    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; private set; } = !string.IsNullOrWhiteSpace(name)
        ? name
        : throw new ArgumentException("Имя не может быть пустым", nameof(name));

    public string Species { get; private set; } = !string.IsNullOrWhiteSpace(species)
        ? species
        : throw new ArgumentException("Вид не может быть пустым", nameof(species));

    public int Energy { get; private set; } = 100;

    public int Intensity { get; private set; }

    public Guid OwnerId { get; private set; } = ownerId;
    public Owner Owner { get; private set; } = null!;

    protected Animal() : this("Internal", "Internal", Guid.Empty)
    {
    }

    public void Feed(int amount) => Energy = amount is < 1 or > 100
        ? throw new ArgumentOutOfRangeException(nameof(amount), "Еда должна быть в диапазоне 1-100")
        : Math.Min(100, Energy + amount);

    public void Play(int intensity)
    {
        if (intensity is < 1 or > MaxIntensity)
            throw new ArgumentOutOfRangeException(nameof(intensity),
                $"Интенсивность должна быть в диапазоне 1-{MaxIntensity}");

        int cost = intensity * 2;

        if (Energy < cost)
            throw new InvalidOperationException("Животное слишком устало для такой активной игры");

        Energy -= cost;

        Intensity = intensity;
    }
}