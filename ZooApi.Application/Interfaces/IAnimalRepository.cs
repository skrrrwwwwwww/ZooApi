namespace ZooApi.Application.Interfaces;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(int id);
    Task<List<Animal>> GetAllAsync();
    Task<int> AddAsync(Animal animal);
    Task UpdateAsync(Animal animal);
    Task DeleteAsync(int id);
}