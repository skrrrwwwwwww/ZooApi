namespace ZooApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController(IAnimalService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Tags("Получить всех животных")]
    [EndpointSummary("Список всех обитателей 📋")]
    [EndpointDescription("Возвращает полный список животных, находящихся в зоопарке. \n\n*Поддерживает актуальные данные о сытости и настроении.*")]
    [ProducesResponseType(typeof(IEnumerable<AnimalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var animals = await service.GetAllAsync();
        return Ok(mapper.Map<List<AnimalDto>>(animals));
    }

    [HttpGet("{id:guid}")]
    [Tags("Получить животное")]
    [EndpointSummary("Карточка животного по ID 🆔")]
    [EndpointDescription("Получение детальной информации о конкретном животном. \n\n> [!NOTE]\n> Нужно передать валидный **GUID** идентификатор.")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var animal = await service.GetByIdAsync(id);
        return animal is not null 
            ? Ok(mapper.Map<AnimalDto>(animal)) 
            : NotFound();
    }
    
    [HttpPost]
    [Tags("Добавление животного")]
    [EndpointSummary("Регистрация нового жильца 🦁")]
    [EndpointDescription(@"
Создает новую запись в базе данных. 

### Правила заполнения:
- **Name**: Кличка (минимум 2 символа).
- **Species**: Биологический вид (например, *Panthera leo*).
- **Age**: Возраст в годах.

После создания животное получает статус `Сытость: 50%`.")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAnimalDto dto)
    {
        var entity = await service.CreateAsync(dto);
        var resultDto = mapper.Map<AnimalDto>(entity);
        
        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id:guid}/feed")]
    [Tags("Кормление и уход")]
    [EndpointSummary("Покормить питомца 🥩")]
    [EndpointDescription("Увеличивает уровень сытости животного. \n\n**Внимание:** Если перекормить, животное может уснуть на долгое время!")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Feed(Guid id, [FromBody] FeedDto dto)
    {
        var updated = await service.FeedAsync(id, dto);
        return updated is not null 
            ? Ok(mapper.Map<AnimalDto>(updated)) 
            : NotFound();
    }
    
    [HttpPut("{id:guid}/play")]
    [Tags("Игра с животным")]
    [EndpointSummary("Поиграть с животным 🎾")]
    [EndpointDescription("Повышает уровень счастья, но снижает энергию. \n\nПараметр **Intensity** влияет на то, как быстро устанет зверь.")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Play(Guid id, [FromBody] PlayDto dto)
    {
        var updated = await service.PlayAsync(id, dto.Intensity);
        return Ok(mapper.Map<AnimalDto>(updated));
    }

    [HttpDelete("{id:guid}")]
    [Tags("Удаление животного")]
    [EndpointSummary("Выписать из зоопарка 🚪")]
    [EndpointDescription("Полное удаление записи о животном. \n\n> [!CAUTION]\n> Это действие необратимо. Все данные о кормлении будут стерты.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
