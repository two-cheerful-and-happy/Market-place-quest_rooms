using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services;

public class MapService : IMapService
{
    private readonly IBaseRepository<Location> _locationRepositor;

    public MapService(IBaseRepository<Location> locationRepositor)
    {
        _locationRepositor = locationRepositor;
    }

    public async Task<BaseResponse<Location>> AddNewLocationAsync(Location location)
    {
        try
        {
            if(await _locationRepositor.Add(location))
                return new BaseResponse<Location> { StatusCode = System.Net.HttpStatusCode.OK };
            return new BaseResponse<Location> { StatusCode = System.Net.HttpStatusCode.BadRequest };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<List<Location>> GetLocationsAsync()
    {
        try
        {
            return await _locationRepositor.Select().ToListAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
