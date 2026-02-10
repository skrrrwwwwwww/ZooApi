namespace ZooApi.Application.DTOs;

public record AnimalDto(Guid Id, 
                        string Name, 
                        string Species, 
                        int Energy,
                        int Intensity);