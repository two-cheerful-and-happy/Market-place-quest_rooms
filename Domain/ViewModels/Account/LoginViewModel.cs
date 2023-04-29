namespace Domain.ViewModels.Account;

public class LoginViewModel
{
    public string Login { get; set;}
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public Role? Role { get; set; }
}
