using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Domain.ViewModels.AdminPanel;
using Domain.ViewModels.Map;
using Domain.ViewModels.OwnerOfRoom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogic.Services;

public class MapService : IMapService
{
    private readonly IBaseRepository<Location> _locationRepositor;
    private readonly IBaseRepository<Comment> _commentRepositor;
    private readonly IBaseRepository<Account> _accountRepositor;
    private readonly IMemoryCache _memoryCache;
    private const string _listKey = "LocationsMapListKey";

    public MapService(
        IBaseRepository<Location> locationRepositor,
        IBaseRepository<Account> accountRepositor,
        IBaseRepository<Comment> commentRepositor,
        IMemoryCache memoryCache)
    {
        _locationRepositor = locationRepositor;
        _accountRepositor = accountRepositor;
        _commentRepositor = commentRepositor;
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

    public List<Location> GetLocations()
    {
        try
        {
            return GetLocationsFromCache();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private byte[] PackingPhoto(IFormFile file)
    {
        using (var binaryReader = new BinaryReader(file.OpenReadStream()))
        {
            return binaryReader.ReadBytes((int)file.Length);
        }
    }

    private List<Location> GetLocationsFromCache()
    {
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        var locationsFromCache = _memoryCache.GetOrCreate(_listKey, entry =>
        {
            entry.SetOptions(cacheOptions);
            return (from location in _locationRepositor.Select().Include(x => x.Author).Where(x => x.LocationConfirmed == true)
                    select new Location
                    {
                        Id = location.Id,
                        Address = location.Address,
                        Description = location.Description,
                        Author = location.Author,
                        Latitude = location.Latitude,
                        LocationConfirmed = location.LocationConfirmed,
                        Longitude = location.Longitude,
                        Name = location.Name
                    }).ToList();
        });
        return locationsFromCache;
    }

    public async Task<BaseResponse<OverviewLocationViewModel>> GetLocationOverviewAsync(string name)
    {
        try
        {
            var result = new OverviewLocationViewModel();

            var location = await _locationRepositor.Select().Where(x => x.Name == name)
                .Include(x => x.Author)
                .FirstOrDefaultAsync();
            var comments = await _commentRepositor.Select().Where(x => x.Location.Id == location.Id).Include(x => x.Account).ToListAsync();
            

            if (location is null)
                return new BaseResponse<OverviewLocationViewModel>
                {
                    Description = "Location did not find",
                    StatusCode = HttpStatusCode.BadGateway
                };
            if(comments.Count != 0)
            {
                int mark = 0;

                foreach (var item in comments)
                {
                    switch (item.Mark)
                    {
                        case Domain.Enums.Mark.Bad:
                            mark += 1;
                            break;
                        case Domain.Enums.Mark.Low:
                            mark += 2;
                            break;
                        case Domain.Enums.Mark.Medium:
                            mark += 3;
                            break;
                        case Domain.Enums.Mark.Good:
                            mark += 4;
                            break;
                        case Domain.Enums.Mark.Perfect:
                            mark += 5;
                            break;
                        default:
                            break;
                    }
                }
                mark /= comments.Count;
                switch (mark)
                {
                    case 0:
                        result.Mark = Domain.Enums.Mark.Bad;
                        break;
                    case 1:
                        result.Mark = Domain.Enums.Mark.Bad;
                        break;
                    case 2:
                        result.Mark = Domain.Enums.Mark.Low;
                        break;
                    case 3:
                        result.Mark = Domain.Enums.Mark.Medium;
                        break;
                    case 4:
                        result.Mark = Domain.Enums.Mark.Good;
                        break;
                    case 5:
                        result.Mark = Domain.Enums.Mark.Perfect;
                        break;
                }
            }
            

            result.AuthorName = location.Author.Login;
            result.Address = location.Address;
            result.Photo = location.Photo;
            result.Name = location.Name;
            result.Description = location.Name;
            result.Comments = comments;
            result.Id = location.Id;


            return new BaseResponse<OverviewLocationViewModel>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception)
        {

            throw;
        }
    }
}
