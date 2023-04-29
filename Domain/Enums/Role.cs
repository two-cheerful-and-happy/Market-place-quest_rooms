using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Role
{
    [Display(Name = "User")]
    User = 0,
    [Display(Name = "Manager")]
    Manager = 1,
    [Display(Name = "Admin")]
    Admin = 2,
}
