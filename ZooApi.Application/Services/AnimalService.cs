using MassTransit;
using ZooApi.Application.Common.Contracts;
using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;

namespace ZooApi.Application.Services;

public class AnimalService(IAnimalRepository repository, 
                           IPublishEndpoint publishEndpoint) 
                           : IAnimalService
{
    public async Task<List<Animal>> GetAllAsync() 
        => await repository.GetAllAsync();

    public async Task<Animal?> GetByIdAsync(int id) 
        => await repository.GetByIdAsync(id);

    public async Task<Animal> CreateAsync(CreateAnimalDto dto)
    {   
        var animal = new Animal(dto.Name, dto.Species);
        var created = await repository.AddAsync(animal);
        await publishEndpoint.Publish(new AnimalCreated(created.Id, created.Name, created.Species));
        await repository.SaveChangesAsync();
        return created;
    }

    public async Task<Animal> FeedAsync(int id, FeedDto dto)
    {
        var animal = await repository.GetByIdAsync(id) 
                     ?? throw new KeyNotFoundException();
        animal.Feed(dto.FoodAmount);
        await repository.UpdateAsync(animal);
        return animal;
    }

    public async Task DeleteAsync(int id) 
        => await repository.DeleteAsync(id);
}