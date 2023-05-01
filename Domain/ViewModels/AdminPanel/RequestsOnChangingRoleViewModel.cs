using Domain.Entities;

namespace Domain.ViewModels.AdminPanel;

public class RequestsOnChangingRoleViewModel
{
    public List<RequestOnChangingRole> RequestOnChangingRoles { get; set; }
    
    public RequestsOnChangingRoleViewModel(
        List<RequestOnChangingRole> requests
        )
    {
        RequestOnChangingRoles = requests;
        
    }
}
