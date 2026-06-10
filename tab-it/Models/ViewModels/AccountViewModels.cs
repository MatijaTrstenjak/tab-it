using System.ComponentModel.DataAnnotations;

namespace tab_it.Models.ViewModels;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required, StringLength(11, MinimumLength = 11)]
    public string OIB { get; set; } = string.Empty;

    [Required, StringLength(13, MinimumLength = 13)]
    public string JMBG { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

public class ExternalRegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(11, MinimumLength = 11)]
    public string OIB { get; set; } = string.Empty;

    [Required, StringLength(13, MinimumLength = 13)]
    public string JMBG { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
