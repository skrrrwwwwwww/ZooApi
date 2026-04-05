namespace ZooApi.Application.Services;

public class AnimalService(
    IZooDbContext context,
    IPublishEndpoint publishEndpoint)
    : IAnimalService
{
    public async Task<PagedResult<Animal>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = context.Animals.AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(a => a.Owner) // В сущности Animal свойство называется Owner (в единственном числе)
            .OrderBy(a => a.Name) // Сортировка по имени животного
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Animal>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<Animal?> GetByIdAsync(Guid id) =>
        await context.Animals.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Animal> CreateAsync(CreateAnimalDto dto)
    {
        var ownerExists = await context.Owners.AnyAsync(o => o.Id == dto.OwnerId);
    
        if (!ownerExists)
        {
            // Выкидываем исключение, которое потом можно перехватить
            throw new KeyNotFoundException($"Владелец с ID {dto.OwnerId} не найден в базе.");
        }

        // 2. Если всё ок — создаем льва
        var animal = new Animal(dto.Name, dto.Species, dto.OwnerId);
        context.Animals.Add(animal);
    
        await context.SaveChangesAsync();
    
        await publishEndpoint.Publish(new AnimalCreated(animal.Id, animal.Name, animal.Species));
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
        var deleteRows = await context.Animals
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();

        if (deleteRows == 0) throw new KeyNotFoundException();
    }
}