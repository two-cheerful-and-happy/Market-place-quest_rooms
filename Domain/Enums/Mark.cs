using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Mark
{
    [Display(Name = "Bad")]
    Bad = 1,
    [Display(Name = "Low")]
    Low = 2,
    [Display(Name = "Medium")]
    Medium = 3,
    [Display(Name = "Good")]
    Good = 4,
    [Display(Name = "Perfect")]
    Perfect = 5,
}