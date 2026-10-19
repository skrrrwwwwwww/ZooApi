using ZooApi.Application.Common;

namespace ZooApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Управление владельцами животных")]
public class OwnersController(IOwnerService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Список владельцев с пагинацией 📋")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        // Валидация входных данных
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize > 100) pageSize = 100; // Ограничиваем сверху для безопасности

        var result = await service.GetAllAsync(pageNumber, pageSize);
    
        // Мапим Items внутри PagedResult
        var dtoItems = mapper.Map<List<OwnerDto>>(result.Items);
    
        return Ok(new PagedResult<OwnerDto>(dtoItems, result.TotalCount, result.PageNumber, result.PageSize));
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Карточка владельца по ID 🆔")]
    [EndpointDescription("Получение детальной информации о владельце. Нужно передать валидный **GUID**.")]
    [ProducesResponseType(typeof(OwnerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var owner = await service.GetByIdAsync(id);
        return owner is not null ? Ok(mapper.Map<OwnerDto>(owner)) : NotFound();
    }

    [HttpPost]
    [EndpointSummary("Регистрация нового владельца 📄")]
    [EndpointDescription("Создает профиль нового владельца в системе.")]
    [ProducesResponseType(typeof(OwnerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateOwnerDto dto)
    {
        var entity = await service.CreateAsync(dto);
        var resultDto = mapper.Map<OwnerDto>(entity);
        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Обновить данные владельца ✏️")]
    [EndpointDescription("Изменение имени или контактной информации владельца.")]
    [ProducesResponseType(typeof(OwnerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] OwnerUpdateInfo dto)
    {
        var updated = await service.UpdateAsync(id, dto);
        return updated is not null ? Ok(mapper.Map<OwnerDto>(updated)) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Удалить владельца 🗑️")]
    [EndpointDescription("Полное удаление владельца из базы. Внимание: это может затронуть связанных животных!")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
