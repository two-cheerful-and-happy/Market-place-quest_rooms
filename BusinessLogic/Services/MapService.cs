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
