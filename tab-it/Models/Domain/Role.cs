namespace tab_it.Models.Domain;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // 1-N: one role can be assigned to many users.
    public List<User> Users { get; set; } = new();
}
