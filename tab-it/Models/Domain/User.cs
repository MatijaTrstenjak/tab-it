namespace tab_it.Models.Domain;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    // 1-N: one user can open many customer tabs.
    public List<CustomerTab> Tabs { get; set; } = new();
}
