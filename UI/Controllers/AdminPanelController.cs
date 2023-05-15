using Domain.Enums;
using Domain.ViewModels.AdditionalViewModel;
using Domain.ViewModels.AdminPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;

namespace UI.Controllers;

[Authorize(Roles = "Admin, Manager")]
public class AdminPanelController : Controller
{
    private readonly IAdminPanelService _adminPanelService;

    public AdminPanelController(
        IAdminPanelService adminPanelService)
    {
        _adminPanelService = adminPanelService;
    }

    [HttpGet]
    public async Task<IActionResult> LocationConfirmPanel(IsConfirmed? confirmed, string? name, string? author,int page = 1)
    {
        NewFilterOfLocationPanel newFilterOfAccount = new(confirmed.ToString(), name, author, page);
        var response = await _adminPanelService.SetLocationPanelViewModel(newFilterOfAccount, false);
        return View(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Panel(Role? role, string? login, int page = 1)
    {
        NewFilterOfAccountPanel newFilterOfAccount = new(role.ToString(), login, page);
        var response = await _adminPanelService.SetPanelViewModel(newFilterOfAccount, false);
        return View(response.Data);
    }

    [HttpGet]
    public async Task<ActionResult> GetUserCard(int id)
    {
        var response = await _adminPanelService.GetAccountFromCacheAsync(id);
        
        return PartialView("AccountCard", response.Data);
    }

    [HttpGet]
    public async Task<ActionResult> GetLocationCard(int id)
    {
        var response = await _adminPanelService.GetLoationFromCacheAsync(id);

        return PartialView("LocationCard", response.Data);
    }

    [HttpGet]
    public async Task<ActionResult> GetDeleteUserCard(int id)
    {
        return PartialView("DeleteAccountCard", id);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRoleOfAccount(AdminPanelAccountViewModel account)
    {
        string message = string.Empty;
        if (ModelState.IsValid)
        {
            var response = await _adminPanelService.ChangeUserRoleAsync(account, User.Identity.Name);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
                return PartialView("PopupWindow", "Role was changed");
            message = response.Description;
        }
        return PartialView("PopupWindow", message);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmLocation(AdminPanelLocationViewModel model)
    {
        string message = string.Empty;
        
        var response = await _adminPanelService.ConfirmLocationAsync(model);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return PartialView("LocationPopupWindow", "Location was confirmed");
        message = response.Description;
        
        return PartialView("LocationPopupWindow", message);
    }

    [HttpGet]
    public IActionResult ConfirmChangingRoleWindow(string id)
    {
        return PartialView("ConfirmChangingRoleWindow", id);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmChangingRole(string id)
    {
        if(ModelState.IsValid)
        {

        }
        return PartialView();
    }


    [HttpGet]
    public async Task<IActionResult> UpdatePanelData()
    {
        NewFilterOfAccountPanel newFilterOfAccount = new(null, null, 1);
        var result = await _adminPanelService.SetPanelViewModel(newFilterOfAccount, true);        
        return View("Panel", result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateLocationData()
    {
        NewFilterOfLocationPanel newFilterOfLocation = new(null,null, null, 1);
        var result =  await _adminPanelService.SetLocationPanelViewModel(newFilterOfLocation, true);
        return View("LocationConfirmPanel", result.Data);
    }
}
