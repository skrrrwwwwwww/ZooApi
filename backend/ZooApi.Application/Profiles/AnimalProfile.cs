namespace ZooApi.Application.Profiles;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<CreateAnimalDto, Animal>();
        CreateMap<Animal, AnimalDto>()
            .ForMember(dest => dest.OwnerName, opt => 
                opt.MapFrom(src => src.Owner != null ? src.Owner.Name : "Без владельца"));

    }
}