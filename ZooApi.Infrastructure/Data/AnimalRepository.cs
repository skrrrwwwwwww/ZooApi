using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ZooApi.Infrastructure.Data;



public class AnimalRepository : IAnimalRepository
{
    private readonly ZooDbContext _context;

    private AnimalRepository(ZooDbContext context) => _context = context;

    public async Task<List<Animal>> GetAllAsync() =>
        await _context.Animals.ToListAsync();

    public async Task<Animal> GetByIdAsync(int id) =>
        await _context.Animals.FindAsync(id);

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
        var animal = await GetByIdAsync(id) ?? throw new KeyNotFoundException();
        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();
    }
}