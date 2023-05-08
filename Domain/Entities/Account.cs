using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Account
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public DateOnly Birthday  { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Role Role { get; set; }
    public bool AccountConfirmed { get; set; }
    public ICollection<Location> LocationsCreatedByAccount { get; set; }
    public ICollection<Comment> CommentsCreatedByAccount { get; set; }
}
