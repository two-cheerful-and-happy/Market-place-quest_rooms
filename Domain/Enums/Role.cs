using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Role
{
    [Display(Name = "User")]
    User = 0,
    [Display(Name = "Owner of rooms")]
    OwnerOfRooms = 1,
    [Display(Name = "Manager")]
    Manager = 2,
    [Display(Name = "Admin")]
    Admin = 3,
}
