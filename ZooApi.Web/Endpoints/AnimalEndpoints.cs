    namespace ZooApi.Web.Endpoints;

    public static class AnimalEndpoints
    {
        public static void MapAnimals(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/animals");

            group.MapGet("/", async (IAnimalService service, AnimalMap map) =>
            {
                var animals = await service.GetAllAsync();
                return Results.Ok(map.ToDtoList(animals)); // Используем типизированный метод
            });

            group.MapGet("/{id}", async (Guid id, IAnimalService service, AnimalMap map) =>
                await service.GetByIdAsync(id) is { } animal 
                    ? Results.Ok(map.To(animal)) 
                    : Results.NotFound());
        
            group.MapPost("/", async (CreateAnimalDto dto, IAnimalService service, AnimalMap map) =>
            {
                // Если сервис принимает DTO, маппинг внутри сервиса. 
                // Если сервис возвращает сущность — маппим здесь:
                var entity = await service.CreateAsync(dto);
                var resultDto = map.To(entity);
                return Results.Created($"/api/animals/{resultDto.Id}", resultDto);
            });
            
            group.MapPut("/{id}/feed", async (Guid id, FeedDto dto, IAnimalService service, AnimalMap map) =>
                await service.FeedAsync(id, dto) is { } updated 
                    ? Results.Ok(map.To(updated))
                    : Results.NotFound());

            group.MapPut("/{id}/play", async (Guid id, PlayDto dto, IAnimalService service, AnimalMap map) =>
            {
                var updated = await service.PlayAsync(id, dto.Intensity);
                return Results.Ok(map.To(updated));
            });
            
            group.MapDelete("/{id}", async (Guid id, IAnimalService service) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            });

        }
    }