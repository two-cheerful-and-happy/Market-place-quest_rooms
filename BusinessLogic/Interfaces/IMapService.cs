using Domain.ViewModels.Map;
using Domain.ViewModels.OwnerOfRoom;

namespace BusinessLogic.Interfaces;

public interface IMapService
{
    List<Location> GetLocations();
    
    Task<BaseResponse<ValidationResult>> AddNewLocationAsync(AddNewLocationViewModel model);

    Task<BaseResponse<OverviewLocationViewModel>> GetLocationOverviewAsync(string name);

}
