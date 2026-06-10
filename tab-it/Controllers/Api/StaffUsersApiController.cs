using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class StaffUsersApiController : ControllerBase
{
    private readonly TabItDbContext _db;

    public StaffUsersApiController(TabItDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffUserDto>>> GetAll([FromQuery] string? q)
    {
        var query = _db.StaffProfiles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(u => u.FirstName.Contains(q) || u.LastName.Contains(q) || u.Username.Contains(q) || u.Email.Contains(q));
        }

        return Ok((await query.ToListAsync()).Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StaffUserDto>> GetById(int id)
    {
        var user = await _db.StaffProfiles.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        return user is null ? NotFound() : Ok(ToDto(user));
    }

    [HttpPost]
    public async Task<ActionResult<StaffUserDto>> Create(StaffUserWriteDto dto)
    {
        var user = new User();
        Apply(user, dto);
        _db.StaffProfiles.Add(user);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, ToDto(user));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StaffUserWriteDto dto)
    {
        var user = await _db.StaffProfiles.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        Apply(user, dto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.StaffProfiles.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static StaffUserDto ToDto(User user) => new(user.Id, user.FirstName, user.LastName, user.Username, user.Email, user.CreatedAt, user.IsActive, user.RoleId);

    private static void Apply(User user, StaffUserWriteDto dto)
    {
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Username = dto.Username;
        user.Email = dto.Email;
        user.PasswordHash = dto.PasswordHash;
        user.CreatedAt = dto.CreatedAt;
        user.IsActive = dto.IsActive;
        user.RoleId = dto.RoleId;
    }
}
