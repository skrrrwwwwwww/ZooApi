namespace ZooApi.Application.Services;

public class AnimalService(IZooDbContext context, 
    IPublishEndpoint publishEndpoint) 
    : IAnimalService
{
    public async Task<List<Animal>> GetAllAsync() 
        => await context.Animals.AsNoTracking().ToListAsync();

    public async Task<Animal?> GetByIdAsync(Guid id) 
        => await context.Animals.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Animal> CreateAsync(CreateAnimalDto dto)
    {   
        var animal = new Animal(dto.Name, dto.Species);
        
        context.Animals.Add(animal);
        
        await publishEndpoint.Publish(new AnimalCreated(animal.Id, animal.Name, animal.Species));
        await context.SaveChangesAsync(); 
        
        return animal;
    }
    
    public async Task<Animal> PlayAsync(Guid id, int intensity)
    {
        var animal = await context.Animals.FindAsync(id) 
                     ?? throw new KeyNotFoundException();

        animal.Play(intensity);
        
        await context.SaveChangesAsync(); 
        return animal;
    }

    public async Task<Animal> FeedAsync(Guid id, FeedDto dto)
    {
        var animal = await context.Animals.FindAsync(id) 
                     ?? throw new KeyNotFoundException();
        
        animal.Feed(dto.FoodAmount);
        
        await context.SaveChangesAsync();
        return animal;
    }

    public async Task DeleteAsync(Guid id)
    {
        await context.Animals
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
    }
}