using ZooApi.Domain.Entities;

namespace ZooApi.Application.Interfaces;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(int id);
    Task<List<Animal>> GetAllAsync();
    Task<Animal> AddAsync(Animal animal);
    Task UpdateAsync(Animal animal);
    Task DeleteAsync(int id);
}