namespace ZooApi.Web.Endpoints;

public static class AnimalEndpoints
{
    public static void MapAnimals(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/animals");
        
        group.MapPost("/", async (CreateAnimalDto dto, IAnimalService service) =>
            {
                var entity = await service.CreateAsync(dto);
                return Results.Created($"/api/animals/{entity.Id}", entity.ToDto());
            })
            .AddEndpointFilter<ValidationFilter<CreateAnimalDto>>();
        
        group.MapPut("/{id}/feed", async (Guid id, FeedDto dto, IAnimalService service) =>
            {
                var updated = await service.FeedAsync(id, dto);
                return updated is not null ? Results.Ok(updated.ToDto()) : Results.NotFound();
            })
            .AddEndpointFilter<ValidationFilter<FeedDto>>();
        
        group.MapPut("/{id}/play", async (Guid id, PlayDto dto, IAnimalService service) =>
            {
                var updated = await service.PlayAsync(id, dto.Intensity);
                return updated is not null ? Results.Ok(updated.ToDto()) : Results.NotFound();
            })
            .AddEndpointFilter<ValidationFilter<PlayDto>>();
        
        group.MapGet("/", async (IAnimalService service) => 
            Results.Ok((await service.GetAllAsync()).Select(x => x.ToDto())));

        group.MapGet("/{id}", async (Guid id, IAnimalService service) =>
            await service.GetByIdAsync(id) is { } animal 
                ? Results.Ok(animal.ToDto()) 
                : Results.NotFound());

        group.MapDelete("/{id}", async (Guid id, IAnimalService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        }); 
    }
}

