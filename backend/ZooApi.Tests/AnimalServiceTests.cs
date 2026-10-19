using MassTransit;
using Microsoft.EntityFrameworkCore;
using ZooApi.Application.Mappers;
using ZooApi.Application.Services;
using ZooApi.Domain.Entities;
using ZooApi.Infrastructure;

namespace ZooApi.Tests;

public class AnimalServiceTests
{
    [Fact]
    public async Task GetByIdAsync_LoadsOwnerForAnimalDtoMapping()
    {
        var options = new DbContextOptionsBuilder<ZooDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ZooDbContext(options);
        var owner = new Owner("Alex", "alex@example.com");
        context.Owners.Add(owner);
        await context.SaveChangesAsync();

        var animal = new Animal("Leo", "Lion", owner.Id);
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        var service = new AnimalService(context, publishEndpoint: null!);

        var result = await service.GetByIdAsync(animal.Id);

        Assert.NotNull(result);
        Assert.Equal(owner.Name, result.Owner.Name);
        Assert.Equal(owner.Name, result.ToDto().OwnerName);
    }
}
