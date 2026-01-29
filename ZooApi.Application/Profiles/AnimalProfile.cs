using AutoMapper;
using ZooApi.Application.DTOs;
using ZooApi.Domain.Entities;

namespace ZooApi.Application.Profiles;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<CreateAnimalDto, Animal>();
        CreateMap<Animal, AnimalDto>();
        CreateMap<FeedDto, Animal>();
    }
}