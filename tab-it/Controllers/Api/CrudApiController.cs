using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tab_it.DAL;

namespace tab_it.Controllers.Api;

[ApiController]
public abstract class CrudApiController<TEntity, TDto, TWriteDto> : ControllerBase
    where TEntity : class, new()
{
    protected readonly TabItDbContext Db;

    protected CrudApiController(TabItDbContext db)
    {
        Db = db;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll([FromQuery] string? q)
    {
        var entities = await ApplySearch(Db.Set<TEntity>().AsNoTracking(), q).ToListAsync();
        return Ok(entities.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public virtual async Task<ActionResult<TDto>> GetById(int id)
    {
        var entity = await Db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        return entity is null ? NotFound() : Ok(ToDto(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create(TWriteDto dto)
    {
        var entity = new TEntity();
        Apply(entity, dto);
        Db.Set<TEntity>().Add(entity);
        await Db.SaveChangesAsync();

        var id = Db.Entry(entity).Property<int>("Id").CurrentValue;
        return CreatedAtAction(nameof(GetById), new { id }, ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public virtual async Task<IActionResult> Update(int id, TWriteDto dto)
    {
        var entity = await Db.Set<TEntity>().FindAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        Apply(entity, dto);
        await Db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await Db.Set<TEntity>().FindAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        if (entity is tab_it.Models.Domain.ISoftDeletable softDeletable)
        {
            softDeletable.IsDeleted = true;
            softDeletable.DeletedAt = DateTime.UtcNow;
        }
        else
        {
            Db.Set<TEntity>().Remove(entity);
        }

        await Db.SaveChangesAsync();
        return NoContent();
    }

    protected virtual IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, string? q) => query;

    protected abstract TDto ToDto(TEntity entity);
    protected abstract void Apply(TEntity entity, TWriteDto dto);
}
