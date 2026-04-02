namespace ZooApi.Application.DTOs;

public record OwnerDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ContactInfo { get; init; } = string.Empty;

    // Список питомцев этого владельца
    public List<AnimalDto> Animals { get; init; } = new();
}