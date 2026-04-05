using ZooApi.Application.Common;

namespace ZooApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Управление обитателями зоопарка")]
public class AnimalsController(IAnimalService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Список всех обитателей 📋")]
    [EndpointDescription("Возвращает полный список животных. Поддерживает актуальные данные.")]
    [ProducesResponseType(typeof(IEnumerable<AnimalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize > 100) pageSize = 100;

        var result = await service.GetAllAsync(pageNumber, pageSize);
    
        var dtoItems = mapper.Map<List<AnimalDto>>(result.Items);
    
        return Ok(new PagedResult<AnimalDto>(dtoItems, result.TotalCount, result.PageNumber, result.PageSize));
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Карточка животного по ID 🆔")]
    [EndpointDescription("Получение детальной информации. Нужно передать валидный **GUID**.")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var animal = await service.GetByIdAsync(id);
        return animal is not null ? Ok(mapper.Map<AnimalDto>(animal)) : NotFound();
    }

    [HttpPost]
    [EndpointSummary("Регистрация нового жильца 🦁")]
    [EndpointDescription("Создает новую запись. Статус `Сытость: 50%`.")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAnimalDto dto)
    {
        var entity = await service.CreateAsync(dto);
        // ВОТ ТУТ: обязательно мапим в DTO, чтобы разорвать связь с Owner
        var resultDto = mapper.Map<AnimalDto>(entity); 
        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id:guid}/feed")]
    [EndpointSummary("Покормить питомца 🥩")]
    [EndpointDescription("Увеличивает уровень сытости. **Внимание:** Перекорм ведет к спячке!")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Feed(Guid id, [FromBody] FeedDto dto)
    {
        var updated = await service.FeedAsync(id, dto);
        return updated is not null ? Ok(mapper.Map<AnimalDto>(updated)) : NotFound();
    }

    [HttpPut("{id:guid}/play")]
    [EndpointSummary("Поиграть с животным 🎾")]
    [EndpointDescription("Повышает счастье, снижает энергию. Параметр **Intensity** влияет на усталость.")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Play(Guid id, [FromBody] PlayDto dto)
    {
        var updated = await service.PlayAsync(id, dto.Intensity);
        return Ok(mapper.Map<AnimalDto>(updated));
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Выписать из зоопарка 🚪")]
    [EndpointDescription("Полное и необратимое удаление записи.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
