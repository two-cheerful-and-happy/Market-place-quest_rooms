using DataAccessLayer.Interfaces;
using Domain.ViewModels.OwnerOfRoom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogic.Services;

public class MapService : IMapService
{
    private readonly IBaseRepository<Location> _locationRepositor;
    private readonly IBaseRepository<Account> _accountRepositor;
    private readonly IMemoryCache _memoryCache;
    private const string _listKey = "AccountsListKey";

    public MapService(
        IBaseRepository<Location> locationRepositor,
        IBaseRepository<Account> accountRepositor,
        IMemoryCache memoryCache)
    {
        _locationRepositor = locationRepositor;
        _accountRepositor = accountRepositor;
        _memoryCache = memoryCache;
    }

    public async Task<BaseResponse<ValidationResult>> AddNewLocationAsync(AddNewLocationViewModel model)
    {
        try
        {
            var location = await _locationRepositor.Select().Where(x => x.Name == model.Name).FirstOrDefaultAsync();
            var user = await _accountRepositor.Select().Where(x => x.Login == model.Login).FirstOrDefaultAsync();
            if (location != null)
                return new BaseResponse<ValidationResult>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Data = new ValidationResult("Location exists with same name", new List<string> { "Name" })
                };

            location = new Location
            {
                Description = model.Description,
                Address = model.Address,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Name = model.Name,
                LocationConfirmed = false,
                Photo = PackingPhoto(model.Photo),
                Author = user
            };

            if (await _locationRepositor.Add(location))
                return new BaseResponse<ValidationResult> { StatusCode = System.Net.HttpStatusCode.OK };
            return new BaseResponse<ValidationResult> 
            { 
                StatusCode = HttpStatusCode.BadRequest,
                Data = new ValidationResult("Location did not added, server  error", new List<string> { "All" })
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<ValidationResult>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = new ValidationResult(ex.Message, new List<string> { "All" })
            };
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

    private byte[] PackingPhoto(IFormFile file)
    {
        using (var binaryReader = new BinaryReader(file.OpenReadStream()))
        {
            return binaryReader.ReadBytes((int)file.Length);
        }
    }
}
