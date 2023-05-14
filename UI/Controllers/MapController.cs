using BusinessLogic.Interfaces;
using Domain.ViewModels.Map;
using System.Net.WebSockets;

namespace UI.Controllers;

public class MapController : Controller
{
    private readonly IMapService _mapService;
    private readonly IAccountService _accountService;

    public MapController(IMapService mapService, IAccountService accountService)
    {
        _mapService = mapService;
        _accountService = accountService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetLocations()
    {
        var response =  _mapService.GetLocations();

        return Json(response);
    }

    [HttpGet]
    public IActionResult Search()
    {
        return PartialView("SearchModal");
    }

    [HttpGet]
    public async Task<IActionResult> LocationView(string name)
    {
        if (name == null)
            return PartialView("PopupWindow", "Error");
        var response = await _mapService.GetLocationOverviewAsync(name);
        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return View(response.Data);
        }
        return PartialView("PopupWindow", "Error");
    }

    [HttpPost]
    public IActionResult Search(SearchViewModel model)
    {
        if (ModelState.IsValid)
        {
            

            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
}
