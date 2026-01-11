using Microsoft.AspNetCore.Mvc;
using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;
using ZooApi.Application.Services;
using ZooApi.Domain.Entities;

namespace ZooApi.Web.Controllers;

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
        _logger.LogInformation("GET все животные");
        return Ok(await _service.GetAllAsync());
    }
    
    [HttpGet("{id}")] 
    public async Task<ActionResult<Animal>> Get(int id)
    {
        _logger.LogInformation("GET животное {Id}", id);
        var animal = await _service.GetByIdAsync(id);
        return animal != null ? Ok(animal) : NotFound();
    }
    
    [HttpPost]
    public async Task<ActionResult<Animal>> Create(CreateAnimalDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _logger.LogInformation("Создано животное {Name}", dto.Name); 
        var animal = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
    }

    [HttpPut("{id}/feed")]
    public async Task<ActionResult<Animal>> Feed(int id, FeedDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var animal = await _service.FeedAsync(id, dto);
            _logger.LogInformation("Покормлено животное {Id}", id); 
            return Ok(animal);
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Удалено животное {Id}", id); 
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}
