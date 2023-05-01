using Microsoft.AspNetCore.Http;

namespace Domain.ViewModels.OwnerOfRoom;

public class AddNewLocationViewModel
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public IFormFileCollection Photos { get; set; }
}
