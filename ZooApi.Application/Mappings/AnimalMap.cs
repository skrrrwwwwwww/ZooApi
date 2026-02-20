namespace ZooApi.Application.Mappings;

[Mapper]
public partial class AnimalMap
{
    public partial Animal ToEntity(CreateAnimalDto dto);
    public partial AnimalDto To(Animal entity);
    public partial List<AnimalDto> ToDtoList(IEnumerable<Animal> entities);
}