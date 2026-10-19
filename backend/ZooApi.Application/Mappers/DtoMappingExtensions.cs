namespace ZooApi.Application.Mappers;

public static class DtoMappingExtensions
{
    public static AnimalDto ToDto(this Animal animal) =>
        new(
            animal.Id,
            animal.Name,
            animal.Species,
            animal.Energy,
            animal.Intensity,
            animal.OwnerId,
            animal.Owner?.Name ?? "Без владельца");

    public static OwnerDto ToDto(this Owner owner) =>
        new()
        {
            Id = owner.Id,
            Name = owner.Name,
            ContactInfo = owner.ContactInfo,
            Animals = owner.Animals.Select(animal => animal.ToDto()).ToList()
        };
}
