using Domain.ViewModels.OwnerOfRoom;
using Microsoft.AspNetCore.Authorization;

namespace UI.Controllers;

[Authorize(Roles = "OwnerOfRooms")]
public class OwnerOfRoomController : Controller
{
    private readonly IMapService _mapService;

    public OwnerOfRoomController(IMapService mapService)
    {
        _mapService = mapService;
    }

    [HttpGet]
    public IActionResult AddNewLocation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewLocation(AddNewLocationViewModel model)
    {
        if(ModelState.IsValid) 
        {
            //await _mapService.AddNewLocationAsync();
        }
        return View();
    }
}
