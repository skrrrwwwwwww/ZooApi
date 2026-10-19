namespace ZooApi.Application.Profiles;

public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        CreateMap<CreateOwnerDto, Owner>();

        CreateMap<OwnerUpdateInfo, Owner>();

        CreateMap<Owner, OwnerDto>()
            .ForMember(dest => dest.Animals, opt => opt.MapFrom(src => src.Animals));
    }
}