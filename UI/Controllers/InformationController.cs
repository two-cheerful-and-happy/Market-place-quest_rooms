namespace UI.Controllers
{
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            return PartialView ();
        }
    }
}
