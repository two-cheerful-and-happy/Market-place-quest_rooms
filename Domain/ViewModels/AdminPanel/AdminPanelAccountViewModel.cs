namespace Domain.ViewModels.AdminPanel;

public class AdminPanelAccountViewModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public Role Role { get; set; }
    public bool AccountConfirmed { get; set; }
}
