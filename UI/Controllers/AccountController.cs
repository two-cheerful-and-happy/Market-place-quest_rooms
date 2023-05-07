using Domain.Helpers;
using Domain.ViewModels.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Domain.ViewModels.Shared;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Azure;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using System.Web.Helpers;
using Newtonsoft.Json;
using Domain.DTO;

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
        
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRegistrationViewModel model)
    {
        if(ModelState.IsValid) 
        {
           var response = await _accountService.LoginAsync(model.Login);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var user = _accountService.GetCookieAccountByLogin(model.Login.Login);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                string jsonCookie = JsonConvert.SerializeObject(user);

                Response.Cookies.Append("UserCookie", jsonCookie);

                return RedirectToAction("Index", "Map");
            }
            ModelState.AddModelError("Login.Password", response.Description);
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

    [HttpGet]
     public IActionResult RequestToChangePassword()
    {
        return PartialView("RequestToChangePassword");
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error", "Link is wrong");
        }
        var user = await _accountService.GetAccountByLoginAsync(userId);
        if (user.Data != null)
        {
            if (HashPasswordHelper.HashPassowrd(user.Data.Login) == code && int.Parse(userId) == user.Data.Id)
                return View(new ChangingAccountPasswordViewModel
                {
                    Id = user.Data.Id
                });
        }
        return View("Error", "Parameters are wrong.");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangingAccountPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountService.ChangePasswordAsync(model);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return PartialView(
                    "SuccessRequestToChangePopup",
                    new PopupWindowViewModel
                    {
                        Title = "Success",
                        Body = $"Password was changed."
                    });
            }
            ModelState.AddModelError(response.Data.MemberNames.First(), response.Data.ErrorMessage);
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendRequestToChangingPassword(FormEmailViewModel model)
    {
        if(ModelState.IsValid)
        {
            var user = await _accountService.GetAccountByEmailAsync(model.Email);
            if (user.StatusCode == HttpStatusCode.OK && user.Data.Email == model.Email)
            {
                var urlLink = Url.Action(
                    "ChangePassword",
                    "Account",
                    new { userId = user.Data.Id, code = HashPasswordHelper.HashPassowrd(user.Data.Login.ToString()) },
                    protocol: HttpContext.Request.Scheme);
                var mail = $"<p>Open next link to change password: {urlLink}</p>";

                await _mailService.SendEmailAsync(model.Email, "Confirm email", mail);
                return PartialView(
                    "SuccessRegistrationPopup",
                    new PopupWindowViewModel
                    {
                        Title = "Success",
                        Body = $"Check your account: {model.Email} to change password."
                    });
            }
            user.Description = "Email does not match";
            ModelState.AddModelError("Email", user.Description);
        }
        return View("RequestToChangePassword", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration(LoginRegistrationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountService.RegistrationAsync(model.Registration);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = await _accountService.GetAccountByLoginAsync(model.Registration.Login);
                user.Data.Login = HashPasswordHelper.HashPassowrd(user.Data.Login);
                var urlLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Data.Id, code = user.Data.Login.ToString() },
                    protocol: HttpContext.Request.Scheme);
                var mail = $"<p>Open next link: {urlLink}</p>";

                await _mailService.SendEmailAsync(model.Registration.Email, "Confirm email", mail);

                return PartialView(
                    "SuccessRegistrationPopup",
                    new PopupWindowViewModel
                    {
                        Title = "Success",
                        Body = $"Check your account: {model.Registration.Email} to confirm your email."
                    });
            }
            foreach (var item in response.Data.MemberNames)
                ModelState.AddModelError(item, response.Data.ErrorMessage);
            
        }
        return View("Login", model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Cookies.Delete("UserCookie");

        return RedirectToAction("Index", "Map");
    }
}
