using Domain.Enums;
using Domain.ViewModels.AdditionalViewModel;
using Domain.ViewModels.AdminPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace UI.Controllers;

//[Authorize( Roles = "Admin, Manager")]
public class AdminPanelController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IAdminPanelService _adminPanelService;

    public AdminPanelController(
        IAccountService accountService, 
        IAdminPanelService adminPanelService)
    {
        _accountService = accountService;
        _adminPanelService = adminPanelService;
    }

    [HttpGet]
    public async Task<IActionResult> Panel(Role? role, string? login, int page = 1)
    {
        NewFilterOfAccountPanel newFilterOfAccount = new(role.ToString(), login, page);
        var response = await _adminPanelService.SetPanelViewModel(newFilterOfAccount);
        var a = response.Data.Accounts.FirstOrDefault();
        return View(response.Data);
    }

    [HttpGet]
    public async Task<ActionResult> GetUserCard(int id)
    {
        var response = await _adminPanelService.GetAccountFromCacheAsync(id);
        
        return PartialView("AccountCard", response.Data);
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
                return PartialView("SuccessPopupWindow", "Role was changed");
            message = response.Description;
        }
        return PartialView("ErrorPopupWindow", message);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmChangingRoleWindow(string id)
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
    public async Task<ActionResult> RequestsOnChangingRole()
    {
        var response = await _adminPanelService.GetRequestsOnChangingRoleAsync();
        if(response.StatusCode == System.Net.HttpStatusCode.OK)
            return View(response.Data);
        return View("Error", response.Data);
    }
}
