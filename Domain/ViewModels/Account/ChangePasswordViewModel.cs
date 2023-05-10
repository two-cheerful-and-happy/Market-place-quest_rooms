using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels.Account;

public class ChangePasswordViewModel
{
    public string Token { get; set; }
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "Password have to be required")]
    [MinLength(8, ErrorMessage = "Password have to be more 8 chars")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Password confirm have to be required")]
    [Compare("NewPassword", ErrorMessage = "Password dosen't compare")]
    public string NewPasswordConfirm { get; set; }
}
