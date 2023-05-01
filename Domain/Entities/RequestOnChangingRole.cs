namespace Domain.Entities;

public class RequestOnChangingRole
{
    public Guid Id { get; set; }
    public Role RequestedRole { get; set; }
    public string? DescriptionOrReason { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
