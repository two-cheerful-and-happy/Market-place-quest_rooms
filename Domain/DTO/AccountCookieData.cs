namespace Domain.DTO;

public class AccountCookieData
{
    public string Email { get; set; }
    public string Login { get; set; }
    public DateOnly Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Role Role { get; set; }
}
