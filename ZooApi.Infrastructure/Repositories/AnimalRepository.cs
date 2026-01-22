using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ZooApi.Infrastructure;

namespace ZooApi.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly ZooDbContext _context;

    public AnimalRepository(ZooDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Animal>> GetAllAsync() =>
        await _context.Animals.AsNoTracking().ToListAsync();

    public async Task<Animal> GetByIdAsync(int id) => 
        await _context.Animals
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    public async Task<Animal> AddAsync(Animal animal)
    {
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();
        return animal;
    }

    public async Task UpdateAsync(Animal animal)
    {
        _context.Entry(animal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var animal = await _context.Animals.FindAsync(id) 
                     ?? throw new KeyNotFoundException();
        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();
    }
}

