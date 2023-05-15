using Domain.Responses;
using Domain.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using System.Web.WebPages;

namespace UI.Controllers;

//[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public IActionResult AddComment(string locationId, string login)
    {
        if(locationId == null || login == null ) 
            return PartialView(
                    "PopupWindow",
                    $"Parametters are wrong.");

        if (!decimal.TryParse(locationId, out decimal locationIdDecimal))
            return PartialView("PopupWindow", "locationId is not a valid decimal number.");

        var model = new AddCommentViewModel
        {
            LocationId = Convert.ToInt32(locationId),
            Name = login
        };
        return PartialView(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(AddCommentViewModel model)
    {
        var result = new BaseResponse<Comment>();
        if (ModelState.IsValid)
        {
            result = await _commentService.AddCommentAsync(model);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return RedirectToAction("LocationView", "Map", new { name = result.Data.Location.Name });
            
        }
        return PartialView("PopupWindow", result.Description);
    }
}
