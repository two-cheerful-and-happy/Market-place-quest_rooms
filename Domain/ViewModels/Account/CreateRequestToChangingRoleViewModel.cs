namespace Domain.ViewModels.Account;

public class CreateRequestToChangingRoleViewModel
{
    public string? Login { get; set; }
    public string Description { get; set; }
    public Role RequestedRole { get; set; }
    public bool IsRepeate { get; set; } = false;
}
