using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace tab_it.Models.Domain;

public class AppUser : IdentityUser, ISoftDeletable
{
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string OIB { get; set; } = string.Empty;

    [Required]
    [StringLength(13, MinimumLength = 13)]
    public string JMBG { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
