namespace Domain.ViewModels.AdditionalViewModel;

public class NewFilterOfAccountPanel
{
    public string Role { get ; set; }
    public string Login { get; set; }
    public int Page { get; set; }

    public NewFilterOfAccountPanel(string role, string login, int page)
    {
        Role = role;
        Login = login;
        Page = page;
    }
}
