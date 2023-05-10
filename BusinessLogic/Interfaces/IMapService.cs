using Domain.ViewModels.OwnerOfRoom;

namespace BusinessLogic.Interfaces;

public interface IMapService
{
    Task<List<Location>> GetLocationsAsync();
    
    Task<BaseResponse<ValidationResult>> AddNewLocationAsync(AddNewLocationViewModel model);

}
