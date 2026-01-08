namespace ZooApi.Application.Interfaces;

public interface IAnimalService
{
    Task<List<Animal>> GetAllAsync();
    Task<Animal?> GetByIdAsync(int id);
    Task<Animal> CreateAsync(CreateAnimalDto dto);
    Task<Animal> FeedAsync(int id, FeedDto dto);
    Task DeleteAsync(int id);
}