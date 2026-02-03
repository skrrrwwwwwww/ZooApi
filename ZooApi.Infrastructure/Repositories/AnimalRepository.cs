using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ZooApi.Infrastructure.Repositories;

public class AnimalRepository(ZooDbContext context) : IAnimalRepository
{
    public async Task<List<Animal>> GetAllAsync() =>
        await context.Animals.AsNoTracking().ToListAsync();

    public async Task<Animal?> GetByIdAsync(int id) => 
        await context.Animals.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    
    public async Task<Animal>  AddAsync(Animal animal)
    {
        context.Animals.Add(animal);
        await context.SaveChangesAsync();
        return animal;
    }

    public async Task UpdateAsync(Animal animal)
    {
        context.Animals.Update(animal);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var rows = await context.Animals.Where(a => a.Id == id).ExecuteDeleteAsync();
        if (rows == 0) throw new KeyNotFoundException();
    }
}

