using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;

namespace ZooApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController(IAnimalService service, IMapper mapper) : ControllerBase
{
    [HttpGet] 
    public async Task<ActionResult<List<AnimalDto>>> GetAll()
    {
        var animals = await service.GetAllAsync();
        return Ok(mapper.Map<List<AnimalDto>>(animals));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalDto>> Get(Guid id)
    {
        var animal = await service.GetByIdAsync(id);
        return animal is null ? NotFound() : Ok(mapper.Map<AnimalDto>(animal));
    }

    [HttpPost]
    public async Task<ActionResult<AnimalDto>> Create(CreateAnimalDto dto)
    {
        var entity = await service.CreateAsync(dto); 
        var resultDto = mapper.Map<AnimalDto>(entity);
        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}/feed")]
    public async Task<ActionResult<AnimalDto>> Feed(Guid id, FeedDto dto)
    {
        var updatedAnimal = await service.FeedAsync(id, dto);
        if (updatedAnimal == null) return NotFound();
        
        return Ok(mapper.Map<AnimalDto>(updatedAnimal));
    }

    [HttpPut("{id}/play")]
    public async Task<ActionResult<AnimalDto>> Play(Guid id, PlayDto dto)
    {
        var updatedAnimal = await service.PlayAsync(id, dto.Intensity);
    
        return Ok(mapper.Map<AnimalDto>(updatedAnimal));
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}