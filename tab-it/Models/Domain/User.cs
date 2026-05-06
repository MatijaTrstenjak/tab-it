using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class User
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }
    public virtual Role? Role { get; set; }

    // 1-N: one user can open many customer tabs.
    public virtual ICollection<CustomerTab> Tabs { get; set; } = new List<CustomerTab>();
}
