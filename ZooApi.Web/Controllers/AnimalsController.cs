using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _service;
    private readonly ILogger<AnimalsController> _logger;

    public AnimalsController(IAnimalService service, ILogger<AnimalsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet] 
    public async Task<ActionResult<List<Animal>>> GetAll()
    {
        _logger.LogInformation("Получен запрос на получение всех животных");
        var animals = await _service.GetAllAsync();
        _logger.LogInformation("Успешно возвращено {Count} животных", animals.Count);
        return Ok(animals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> Get(int id)
    {
        _logger.LogInformation("Получен запрос на получение животного с ID: {Id}", id);
        var animal = await _service.GetByIdAsync(id);
        
        if (animal == null)
        {
            _logger.LogWarning("Животное с ID {Id} не найдено", id);
            return NotFound();
        }
        
        _logger.LogInformation("Успешно возвращено животное с ID: {Id}", id);
        return Ok(animal);
    }

    [HttpPost]
    public async Task<ActionResult<Animal>> Create(CreateAnimalDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидная модель для создания животного: {Errors}", 
                string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Получен запрос на создание животного: {@AnimalDto}", dto);
        var animal = await _service.CreateAsync(dto);
        _logger.LogInformation("Успешно создано животное с ID: {Id}", animal.Id);
        return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
    }

    [HttpPut("{id}/feed")]
    public async Task<ActionResult<Animal>> Feed(int id, FeedDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидная модель для кормления животного ID {Id}: {Errors}", id,
                string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Получен запрос на кормление животного ID {Id}: {@FeedDto}", id, dto);
        
        try
        {
            var animal = await _service.FeedAsync(id, dto);
            _logger.LogInformation("Успешно покормлено животное с ID: {Id}", id);
            return Ok(animal);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Животное с ID {Id} не найдено при попытке кормления", id);
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Получен запрос на удаление животного с ID: {Id}", id);
        
        try
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Успешно удалено животное с ID: {Id}", id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Животное с ID {Id} не найдено при попытке удаления", id);
            return NotFound();
        }
    }
}
