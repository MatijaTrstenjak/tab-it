using System.ComponentModel.DataAnnotations;

namespace tab_it.Models.Domain;

public class Role
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // 1-N: one role can be assigned to many users.
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
