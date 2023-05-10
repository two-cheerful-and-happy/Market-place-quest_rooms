using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum IsConfirmed
{
    [Display(Name = "Is not confirmed")]
    IsNotConfirmed,
    [Display(Name = "Is confirmed")]
    IsConfirmed
}