using Domain.Entities;

namespace Domain.ViewModels.Account;

public class ProfileViewModel
{
    public List<Location> Locations { get; set; }
    public List<Comment> Comments { get; set; }

}
