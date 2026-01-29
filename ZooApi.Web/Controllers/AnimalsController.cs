using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;

namespace ZooApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController(IAnimalService service, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    [HttpGet] 
    public async Task<ActionResult<List<AnimalDto>>> GetAll()
    {
        var animals = await service.GetAllAsync();
        return Ok(_mapper.Map<List<AnimalDto>>(animals));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> Get(int id)
    {
        var animal = await service.GetByIdAsync(id);
        return animal is null ? NotFound() : Ok(mapper.Map<AnimalDto>(animal));
    }

    [HttpPost]
    public async Task<ActionResult<AnimalDto>> Create(CreateAnimalDto dto)
    {
        var entity = await service.CreateAsync(dto); 
        var resultDto = _mapper.Map<AnimalDto>(entity);
        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}/feed")]
    public async Task<ActionResult<Animal>> Feed(int id, FeedDto dto)
    {
        var updatedAnimal= Ok(await service.FeedAsync(id, dto));
        return Ok(mapper.Map<AnimalDto>(updatedAnimal));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
} 