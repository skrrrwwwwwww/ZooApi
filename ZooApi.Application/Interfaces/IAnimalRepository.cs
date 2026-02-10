using ZooApi.Domain.Entities;

namespace ZooApi.Application.Interfaces;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(Guid id);
    Task<List<Animal>> GetAllAsync();
    Task<Animal> AddAsync(Animal animal);
    Task SaveChangesAsync();
    Task UpdateAsync(Animal animal);
    Task DeleteAsync(Guid id);
}