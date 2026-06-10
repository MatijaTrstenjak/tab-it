using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Admin")]
public class StaffRolesApiController : ControllerBase
{
    private readonly TabItDbContext _db;

    public StaffRolesApiController(TabItDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffRoleDto>>> GetAll([FromQuery] string? q)
    {
        var query = _db.StaffRoles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(r => r.Name.Contains(q) || r.Description.Contains(q));
        }

        return Ok((await query.ToListAsync()).Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StaffRoleDto>> GetById(int id)
    {
        var role = await _db.StaffRoles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        return role is null ? NotFound() : Ok(ToDto(role));
    }

    [HttpPost]
    public async Task<ActionResult<StaffRoleDto>> Create(StaffRoleWriteDto dto)
    {
        var role = new Role();
        Apply(role, dto);
        _db.StaffRoles.Add(role);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, ToDto(role));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StaffRoleWriteDto dto)
    {
        var role = await _db.StaffRoles.FindAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        Apply(role, dto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _db.StaffRoles.FindAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static StaffRoleDto ToDto(Role role) => new(role.Id, role.Name, role.Description);

    private static void Apply(Role role, StaffRoleWriteDto dto)
    {
        role.Name = dto.Name;
        role.Description = dto.Description;
    }
}
