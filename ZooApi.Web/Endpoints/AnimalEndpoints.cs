namespace ZooApi.Web.Endpoints;

public static class AnimalEndpoints
{
    public static void MapAnimals(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/animals");

        group.MapGet("/", async (IAnimalService service, IMapper mapper) =>
        {
            var animals = await service.GetAllAsync();
            return Results.Ok(mapper.Map<List<AnimalDto>>(animals));
        });

        group.MapGet("/id", async (Guid id, IAnimalService service, IMapper mapper) =>
            await service.GetByIdAsync(id) is { } animal 
                ? Results.Ok(mapper.Map<AnimalDto>(animal)) 
                : Results.NotFound());
        
        group.MapPost("/", async (CreateAnimalDto dto, IAnimalService service, IMapper mapper) =>
        {
            var entity = await service.CreateAsync(dto);
            var resultDto = mapper.Map<AnimalDto>(entity);
            return Results.Created($"/api/animals/{resultDto.Id}", resultDto);
        });
        
        group.MapPut("/{id}/feed", async (Guid id, FeedDto dto, IAnimalService service, IMapper mapper) =>
            await service.FeedAsync(id, dto) is { } updated 
                ? Results.Ok(mapper.Map<AnimalDto>(updated))
                : Results.NotFound());
        
        group.MapPut("/{id}/play", async (Guid id, PlayDto dto, IAnimalService service, IMapper mapper) =>
        {
            var updated = await service.PlayAsync(id, dto.Intensity);
            return Results.Ok(mapper.Map<AnimalDto>(updated));
        });
        
        group.MapDelete("/{id}", async (Guid id, IAnimalService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}