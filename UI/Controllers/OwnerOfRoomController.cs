using Domain.ViewModels.OwnerOfRoom;

namespace UI.Controllers;

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
