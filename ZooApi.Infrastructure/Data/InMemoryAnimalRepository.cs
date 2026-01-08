namespace ZooApi.Infrastructure.Data;

public class InMemoryAnimalRepository : IAnimalRepository
{
    private static readonly List<Animal> _animals = new();
    private static int _nextId = 1;
    private readonly object _lock = new();

    public async Task<List<Animal>> GetAllAsync()
        => await Task.FromResult(_animals.ToList());

    public async Task<Animal?> GetByIdAsync(int id)
        => await Task.FromResult(_animals.FirstOrDefault(a => a.Id == id));

    public async Task<int> AddAsync(Animal animal)
    {
        lock (_lock)
        {
            animal.Id = _nextId++;
            _animals.Add(animal);
            return animal.Id;
        }
    }

    public async Task UpdateAsync(Animal animal)
        => await Task.CompletedTask;

    public async Task DeleteAsync(int id)
    {
        lock (_lock)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id)
                         ?? throw new KeyNotFoundException();
            _animals.Remove(animal);
        }

        await Task.CompletedTask;
    }
}