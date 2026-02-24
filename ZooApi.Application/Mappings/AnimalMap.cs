namespace ZooApi.Application.Mappings;

[Mapper]
public static partial class AnimalMap
{
    public static partial AnimalDto ToDto(this Animal entity);
    public static partial Animal ToEntity(this CreateAnimalDto dto);
    public static partial List<AnimalDto> ToDtoList(this IEnumerable<Animal> entities);
}