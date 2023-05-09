using Domain.Helpers;
using Domain.ViewModels.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Domain.ViewModels.Shared;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Domain.DTO;
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
        
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ChangeEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error", "Link is wrong");
        }
        var result = new ChangeEmailViewModel
        {
            Id = Convert.ToInt32(userId),
            Token = code
        };
        return PartialView(result);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeLogin(ChangeLoginViewModel model)
    {
        var error = new SettingViewModel();
        if (ModelState.IsValid)
        {
            var response = await _accountService.ChangeLoginAsync(model);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                string jsonCookie = JsonConvert.SerializeObject(response.Data);
                Response.Cookies.Delete("UserCookie");
                Response.Cookies.Append("UserCookie", jsonCookie);
                return PartialView(
                    "SuccessPopupWindow",
                    $"Login was changed.");

            }

            error.LoginError = response.Description;
        }
        return View("Setting", error);
    }

    [HttpGet]
    public IActionResult ChangeLoginError(string code)
    {
        var result = new ChangeLoginViewModel
        {
            Token = code
        };
        ModelState.AddModelError("Login", code);
        return PartialView("ChangeLogin", result);
    }

    [HttpGet]
    public IActionResult ChangePasswordError(string code)
    {
        var result = new ChangePasswordViewModel
        {
            Token = code
        };
        ModelState.AddModelError("All", code);

        return PartialView("ChangePasswordInAccount", result);
    }

    [HttpGet]
    public IActionResult ChangeLogin(string code)
    {
        if (code == null)
        {
            return View("Error", "Link is wrong");
        }
        var result = new ChangeLoginViewModel
        {
            Token = code
        };
        return PartialView(result);
    }

    [HttpGet]
    public IActionResult ChangePasswordInAccount(string code)
    {
        if (code == null)
        {
            return View("Error", "Link is wrong");
        }
        var result = new ChangePasswordViewModel
        {
            Token = code
        };
        return PartialView(result);

    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordInAccount(ChangePasswordViewModel model)
    {
        var error = new SettingViewModel();
        if (ModelState.IsValid)
        {
            
            var response = await _accountService.ChangePasswordAsync(model);
            if(response.StatusCode == HttpStatusCode.OK)
                return PartialView(
                    "SuccessPopupWindow",
                    $"Password was changed.");
            error.PasswordError = response.Data.ErrorMessage;
        }
        return View("Setting", error);

    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountService.ChangeEmailAsync(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonCookie = JsonConvert.SerializeObject(response.Data);
                Response.Cookies.Delete("UserCookie");
                Response.Cookies.Append("UserCookie", jsonCookie);

                var urlLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = model.Id, code = model.Token },
                    protocol: HttpContext.Request.Scheme);
                var mail = $"<p>Open next link to confirm email: {urlLink}</p>";

                await _mailService.SendEmailAsync(model.Email, "Confirm email", mail);

                return PartialView(
                "SuccessPopupWindow",
                $"Check your account: {model.Email} to confirm email.");
            }
            
            ModelState.AddModelError("Email", response.Description);
        }
        ModelState.AddModelError("Email", "Parametters is wrong");
        return PartialView("ChangeEmail", model);

    }

    [HttpGet]
    public IActionResult ModalRequestToChangeEmail()
    {
        var userCookieJson = Request.Cookies["UserCookie"];
        var userCookie = JsonConvert.DeserializeObject<AccountCookieData>(userCookieJson);
        var model = new FormEmailViewModel
        {
            Email = userCookie.Email
        };
        return PartialView(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendRequestToChangeEmail(FormEmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _accountService.GetAccountByEmailAsync(model.Email);

            var urlLink = Url.Action(
                    "ChangeEmail",
                    "Account",
                    new { userId = user.Data.Id, code = HashPasswordHelper.HashPassowrd(user.Data.Login.ToString()) },
                    protocol: HttpContext.Request.Scheme);
            var mail = $"<p>Open next link to change Email: {urlLink}</p>";

            await _mailService.SendEmailAsync(model.Email, "Confirm email", mail);
            return PartialView(
                "SuccessPopupWindow",
                $"Check your account: {model.Email} to change password.");
        }
        return PartialView(model);
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
            return RedirectToAction("Index", "Map");
        else
            return View("Error", "User don't exist");
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult Setting()
    {
        return View(new SettingViewModel());
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
                return PartialView(new ChangingAccountPasswordViewModel
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
        return PartialView(model);
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
            model.Description = "Email does not match";
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

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Cookies.Delete("UserCookie");

        return RedirectToAction("Index", "Map");
    }
}
