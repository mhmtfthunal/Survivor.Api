using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Api.Data;
using Survivor.Api.Dtos;
using Survivor.Api.Entities;

namespace Survivor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly SurvivorDbContext _db;
    public CategoriesController(SurvivorDbContext db) => _db = db;

    // GET /api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        => await _db.Categories.AsNoTracking().ToListAsync();

    // GET /api/categories/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var cat = await _db.Categories
            .Include(c => c.Competitors)
            .FirstOrDefaultAsync(c => c.Id == id);

        return cat is null ? NotFound() : cat;
    }

    // POST /api/categories
    [HttpPost]
    public async Task<ActionResult<Category>> Create(CategoryCreateUpdateDto dto)
    {
        var entity = new Category { Name = dto.Name };
        _db.Categories.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    // PUT /api/categories/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryCreateUpdateDto dto)
    {
        var entity = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        entity.Name = dto.Name;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/categories/{id}  (soft delete)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        entity.IsDeleted = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
