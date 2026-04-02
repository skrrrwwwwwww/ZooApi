namespace ZooApi.Domain.Entities;

public class Owner
{
    public Guid Id { get; init; }
        
    public string Name { get; private set; }
        
    public string ContactInfo { get; private set; }

    private readonly List<Animal> _animals = new();
    public IReadOnlyCollection<Animal> Animals => _animals.AsReadOnly();
        
    protected Owner() : this("Internal", "Internal") { }

    public Owner(string name, string contactInfo)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым", nameof(name));
            
        if (string.IsNullOrWhiteSpace(contactInfo))
            throw new ArgumentException("Контактная информация не может быть пустой", nameof(contactInfo));
            
        Name = name;
        ContactInfo = contactInfo;
    }
    public void AddAnimal(Animal animal)
    {
        if (animal is null)
            throw new ArgumentNullException(nameof(animal), "Животное не может быть пустым"); 
        _animals.Add(animal);
    }
}