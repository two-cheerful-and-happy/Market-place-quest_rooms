using BusinessLogic.Interfaces;

namespace UI.Controllers;

public class MapController : Controller
{
    private readonly IMapService _mapService;

    public MapController(IMapService mapService)
    {
        _mapService = mapService;
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
}
