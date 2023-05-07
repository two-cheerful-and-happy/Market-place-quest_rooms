using DataAccessLayer.Interfaces;
using Domain.ViewModels.OwnerOfRoom;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogic.Services;

public class MapService : IMapService
{
    private readonly IBaseRepository<Location> _locationRepositor;
    private readonly IMemoryCache _memoryCache;
    private const string _listKey = "AccountsListKey";

    public MapService(
        IBaseRepository<Location> locationRepositor,
        IMemoryCache memoryCache)
    {
        _locationRepositor = locationRepositor;
        _memoryCache = memoryCache;
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

    public Location CreateNewLocato(AddNewLocationViewModel location)
    {
        return new Location 
        {
            Name = location.Name,
            Description = location.Description,
            Address = location.Address,
            LocationConfirmed = false,
            
        };   
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

    private void SetToCacheLocations()
    {

    }
}
