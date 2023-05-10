using Domain.DTO;
using Domain.ViewModels.OwnerOfRoom;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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
    public async Task<IActionResult> CreateLocation(AddNewLocationViewModel model)
    {
        if(ModelState.IsValid) 
        {
            var userCookieJson = Request.Cookies["UserCookie"];
            var userCookie = JsonConvert.DeserializeObject<AccountCookieData>(userCookieJson);
            model.Login = userCookie.Login;
            var response = await _mapService.AddNewLocationAsync(model);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return PartialView(
                "PopupWindow",
                $"Location was created, you have to wait when your location will be confirmed");

            foreach (var item in response.Data.MemberNames)
                ModelState.AddModelError(item, response.Data.ErrorMessage);
        }
        return View("AddNewLocation", model);
    }
}
