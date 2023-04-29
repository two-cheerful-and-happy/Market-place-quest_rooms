namespace Domain.DTO;

public class AuthorizationModelDTO 
{
    public string Login { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}