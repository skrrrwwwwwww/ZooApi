using ZooApi.Application.DTOs;
using ZooApi.Domain.Entities;

namespace ZooApi.Application.Interfaces;

public interface IAnimalService
{
    Task<List<Animal>> GetAllAsync();
    Task<Animal?> GetByIdAsync(Guid id);
    Task<Animal> CreateAsync(CreateAnimalDto dto);
    Task<Animal> FeedAsync(Guid id, FeedDto dto);
    Task<Animal> PlayAsync(Guid id, int intensity);
    Task DeleteAsync(Guid id);
}