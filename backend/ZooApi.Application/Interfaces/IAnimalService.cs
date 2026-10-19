namespace ZooApi.Application.Interfaces;

public interface IAnimalService
{
    Task<PagedResult<Animal>> GetAllAsync(int pageNumber, int pageSize);
    Task<Animal?> GetByIdAsync(Guid id);
    Task<Animal> CreateAsync(CreateAnimalDto dto);
    Task<Animal> FeedAsync(Guid id, FeedDto dto);
    Task<Animal> PlayAsync(Guid id, int intensity);
    Task DeleteAsync(Guid id);
}