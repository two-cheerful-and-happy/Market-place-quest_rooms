using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels.Account;

public class RegistrationViewModel
{
    [Required(ErrorMessage = "Email have to be required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Login have to be required")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password have to be required")]
    [MinLength(8, ErrorMessage = "Password have to be more 8 chars")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirm have to be required")]
    [Compare("Password", ErrorMessage = "Password dosen't compare")]
    public string PasswordConfirm { get; set; }

    public Role Role { get; set; }

    public List<ValidationResult> AdditionalErrors { get; set; } = new List<ValidationResult>();
}
