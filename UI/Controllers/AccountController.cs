using Domain.Helpers;
using Domain.ViewModels.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Domain.ViewModels.Shared;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Azure;

namespace UI.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IMailService _mailService;

    public AccountController(
        IAccountService accountService, 
        IMailService mailService)
    {
        _accountService = accountService;
        _mailService = mailService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var response = await _accountService.GetAccountByLoginAsync(User.Identity.Name);
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(ModelState.IsValid) 
        {
           var response = await _accountService.LoginAsync(model);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                return RedirectToAction("Index", "Map");
            }
            ModelState.AddModelError("Password", response.Description);
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {

        if (userId == null || code == null)
        {
            return View("Error", "Link is wrong");
        }
        var result = await _accountService.ConfirmAccountAsync(Convert.ToInt32(userId), code);
        if (result.StatusCode == HttpStatusCode.OK)
            return RedirectToAction("Login", "Account");
        else
            return View("Error", "User don't exist");
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration(RegistrationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountService.RegistrationAsync(model);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = await _accountService.GetAccountByLoginAsync(model.Login);
                user.Data.Login = HashPasswordHelper.HashPassowrd(user.Data.Login);
                var urlLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Data.Id, code = user.Data.Login.ToString() },
                    protocol: HttpContext.Request.Scheme);
                var mail = $"<p>Open next link: {urlLink}</p>";

                await _mailService.SendEmailAsync(model.Email, "Confirm email", mail);

                return PartialView(
                    "SuccessRegistrationPopup",
                    new PopupWindowViewModel
                    {
                        Title = "Success",
                        Body = $"Check your account: {model.Email} to confirm your email."
                    });
            }
            ModelState.AddModelError(response.Data.MemberNames.First(), response.Data.ErrorMessage);
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
