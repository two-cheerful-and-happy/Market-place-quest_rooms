using Domain.ViewModels.OwnerOfRoom;

namespace UI.Controllers;

public class OwnerOfRoomController : Controller
{
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

        }
        return View();
    }
}
