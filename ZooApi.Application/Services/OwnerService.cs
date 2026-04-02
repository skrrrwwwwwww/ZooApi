namespace ZooApi.Application.Services;

public class OwnerService(
    IZooDbContext context,
    IPublishEndpoint publishEndpoint)
    : IOwnerService
{
    public async Task<PagedResult<Owner>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = context.Owners.AsNoTracking();

        var totalCount = await query.CountAsync();
    
        var items = await query
            .Include(o => o.Animals)
            .OrderBy(o => o.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Owner>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<Owner?> GetByIdAsync(Guid id) =>
        await context.Owners
            .Include(o => o.Animals)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Owner> CreateAsync(CreateOwnerDto dto)
    {
        var owner = new Owner(dto.Name, dto.ContactInfo);
        
        context.Owners.Add(owner);
        
        // Публикуем событие создания (если нужно для интеграций)
        await publishEndpoint.Publish(new OwnerCreated(owner.Id, owner.Name));
        
        await context.SaveChangesAsync();
        return owner;
    }

    public async Task<Owner?> UpdateAsync(Guid id, OwnerUpdateInfo dto)
    {
        var owner = await context.Owners.FindAsync(id) 
                    ?? throw new KeyNotFoundException("Владелец не найден");
        
        await context.SaveChangesAsync();
        return owner;
    }

    public async Task DeleteAsync(Guid id)
    {
        var deletedRows = await context.Owners
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();

        if (deletedRows == 0) 
            throw new KeyNotFoundException("Владелец не найден");
    }
}