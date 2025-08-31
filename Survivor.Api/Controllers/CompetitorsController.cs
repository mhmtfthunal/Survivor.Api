using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Api.Data;
using Survivor.Api.Dtos;
using Survivor.Api.Entities;

namespace Survivor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitorsController : ControllerBase
{
    private readonly SurvivorDbContext _db;
    public CompetitorsController(SurvivorDbContext db) => _db = db;

    // GET /api/competitors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Competitor>>> GetAll()
        => await _db.Competitors.Include(x => x.Category).AsNoTracking().ToListAsync();

    // GET /api/competitors/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Competitor>> GetById(int id)
    {
        var comp = await _db.Competitors
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        return comp is null ? NotFound() : comp;
    }

    // GET /api/competitors/categories/{categoryId}
    [HttpGet("categories/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<Competitor>>> GetByCategory(int categoryId)
        => await _db.Competitors
            .Where(x => x.CategoryId == categoryId)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    // POST /api/competitors
    [HttpPost]
    public async Task<ActionResult<Competitor>> Create(CompetitorCreateUpdateDto dto)
    {
        var exists = await _db.Categories.AnyAsync(x => x.Id == dto.CategoryId);
        if (!exists) return BadRequest($"Category {dto.CategoryId} bulunamadı.");

        var entity = new Competitor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            CategoryId = dto.CategoryId
        };

        _db.Competitors.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    // PUT /api/competitors/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CompetitorCreateUpdateDto dto)
    {
        var entity = await _db.Competitors.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        var catExists = await _db.Categories.AnyAsync(x => x.Id == dto.CategoryId);
        if (!catExists) return BadRequest($"Category {dto.CategoryId} bulunamadı.");

        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.CategoryId = dto.CategoryId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/competitors/{id} (soft delete)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Competitors.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        entity.IsDeleted = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
