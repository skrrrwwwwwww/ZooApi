namespace ZooApi.Application.DTOs;

public record AnimalDto(
    Guid Id,
    string Name,
    string Species,
    int Energy,
    int Intensity,
    Guid OwnerId, // ID владельца для ссылок
    string OwnerName // Имя владельца для отображения в списке
);