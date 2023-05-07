using Domain.ViewModels.OwnerOfRoom;

namespace BusinessLogic.Interfaces;

public interface IMapService
{
    Task<List<Location>> GetLocationsAsync();
    
    Task<BaseResponse<Location>> AddNewLocationAsync(Location location);

    Location CreateNewLocato(AddNewLocationViewModel location);
}
