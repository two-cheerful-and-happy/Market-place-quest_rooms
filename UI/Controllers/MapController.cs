using BusinessLogic.Interfaces;
using Domain.ViewModels.Map;

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
    public async Task<IActionResult> GetLocations()
    {
        var response = await  _mapService.GetLocationsAsync();

        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var user = await _accountService.GetAccountByLoginAsync("Goose");
        var a = new Location()
        {
            Address = "s",
            Author = user.Data,
            Name = "s",
            
            Latitude = 48.837930,
            Longitude = 27.107993
        };
        
        return View();
    }

    public IActionResult GetMyObjects()
    {
        var myObjects = new List<Account>
    {
        new Account { Id = 1, Email = "Object 1" },
        new Account { Id = 2, Email = "Object 2" },
        new Account { Id = 3, Email = "Object 3" }
    };

        return Json(myObjects);
    }

    [HttpGet]
    public IActionResult Search()
    {
        return PartialView("SearchModal");
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
