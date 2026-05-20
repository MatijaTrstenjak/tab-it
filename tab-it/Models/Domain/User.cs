using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    [ForeignKey("Role")]
    [Range(1, int.MaxValue)]
    public int RoleId { get; set; }
    public virtual Role? Role { get; set; }

    // 1-N: one user can open many customer tabs.
    public virtual ICollection<CustomerTab> Tabs { get; set; } = new List<CustomerTab>();
}
