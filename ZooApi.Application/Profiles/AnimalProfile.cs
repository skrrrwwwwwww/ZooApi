namespace ZooApi.Application.Profiles;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<CreateAnimalDto, Animal>();
        CreateMap<Animal, AnimalDto>();
    }
}