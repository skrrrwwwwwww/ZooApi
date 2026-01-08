using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;

namespace ZooApi.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _repository;

    public AnimalService(IAnimalRepository repository) 
        => _repository = repository;

    public async Task<List<Animal>> GetAllAsync() 
        => await _repository.GetAllAsync();

    public async Task<Animal?> GetByIdAsync(int id) 
        => await _repository.GetByIdAsync(id);

    public async Task<Animal> CreateAsync(CreateAnimalDto dto)
    {
        var animal = new Animal(dto.Name, dto.Species);
        await _repository.AddAsync(animal);
        return animal;
    }

    public async Task<Animal> FeedAsync(int id, FeedDto dto)
    {
        var animal = await _repository.GetByIdAsync(id) 
                     ?? throw new KeyNotFoundException();
        animal.Feed(dto.FoodAmount);
        await _repository.UpdateAsync(animal);
        return animal;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}