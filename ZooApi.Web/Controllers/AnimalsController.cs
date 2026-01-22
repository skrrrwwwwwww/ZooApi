using Microsoft.AspNetCore.Mvc;
using ZooApi.Application.DTOs;
using ZooApi.Application.Interfaces;
using ZooApi.Domain.Entities;

namespace ZooApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController(IAnimalService service) : ControllerBase
{
    [HttpGet] 
    public async Task<ActionResult<List<Animal>>> GetAll() 
        => Ok(await service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> Get(int id)
    {
        var animal = await service.GetByIdAsync(id);
        return animal is null ? NotFound() : Ok(animal);
    }

    [HttpPost]
    public async Task<ActionResult<Animal>> Create(CreateAnimalDto dto)
    {
        var animal = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
    }

    [HttpPut("{id}/feed")]
    public async Task<ActionResult<Animal>> Feed(int id, FeedDto dto)
        => Ok(await service.FeedAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}