namespace ZooApi.Application.Interfaces;

public interface IOwnerService
{
    Task<PagedResult<Owner>> GetAllAsync(int pageNumber, int pageSize);
    Task<Owner?> GetByIdAsync(Guid id);
    Task<Owner> CreateAsync(CreateOwnerDto dto); 
    Task<Owner?> UpdateAsync(Guid id, OwnerUpdateInfo dto); 
    Task DeleteAsync(Guid id); 
}