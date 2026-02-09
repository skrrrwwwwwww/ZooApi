using ZooApi.Application.DTOs;
using ZooApi.Domain.Entities;

namespace ZooApi.Application.Interfaces;

public interface IAnimalService
{
    Task<List<Animal>> GetAllAsync();
    Task<Animal?> GetByIdAsync(int id);
    Task<Animal> CreateAsync(CreateAnimalDto dto);
    Task<Animal> FeedAsync(int id, FeedDto dto);
    Task<Animal> PlayAsync(int id, int intensity);
    Task DeleteAsync(int id);
}