using System.ComponentModel.DataAnnotations;

namespace tab_it.Models.ViewModels;

public record IdentityUserListItemViewModel(
    string Id,
    string Email,
    string UserName,
    string OIB,
    string JMBG,
    bool EmailConfirmed,
    IReadOnlyList<string> Roles);

public class IdentityUserEditViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required, StringLength(11, MinimumLength = 11)]
    public string OIB { get; set; } = string.Empty;

    [Required, StringLength(13, MinimumLength = 13)]
    public string JMBG { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }
    public List<string> SelectedRoles { get; set; } = new();
    public List<string> AvailableRoles { get; set; } = new();
}

public record IdentityRoleListItemViewModel(string Id, string Name);

public class IdentityRoleEditViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;
}
