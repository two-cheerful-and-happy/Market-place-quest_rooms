using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels.Account;

public class ChangingAccountPasswordViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Password have to be required")]
    [MinLength(8, ErrorMessage = "Password have to be more 8 chars")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirm have to be required")]
    [Compare("Password", ErrorMessage = "Password dosen't compare")]
    public string PasswordConfirm { get; set; }
}
