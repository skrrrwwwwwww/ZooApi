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

    public async Task<Animal?> GetByIdAsync(Guid id) 
        => await repository.GetByIdAsync(id);

    public async Task<Animal> CreateAsync(CreateAnimalDto dto)
    {   
        var animal = new Animal(dto.Name, dto.Species);
        await repository.AddAsync(animal);
        
        await publishEndpoint.Publish(new AnimalCreated(animal.Id, animal.Name, animal.Species));
    
        await repository.SaveChangesAsync(); 

        return animal;
    }
    
    public async Task<Animal> PlayAsync(Guid id, int intensity)
    {
        var animal = await repository.GetByIdAsync(id) 
                     ?? throw new KeyNotFoundException();

        animal.Play(intensity);

        await repository.UpdateAsync(animal);
        return animal;
    }

    public async Task<Animal> FeedAsync(Guid id, FeedDto dto)
    {
        var animal = await repository.GetByIdAsync(id) 
                     ?? throw new KeyNotFoundException();
        animal.Feed(dto.FoodAmount);
        await repository.UpdateAsync(animal);
        return animal;
    }

    public async Task DeleteAsync(Guid id) 
        => await repository.DeleteAsync(id);
}