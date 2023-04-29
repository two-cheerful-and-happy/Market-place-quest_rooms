using DataAccessLayer.Interfaces;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BusinessLogic.Services;

public class AccountService : IAccountService
{
    private readonly IBaseRepository<Account> _accountRepository;
    private readonly IBaseRepository<Location> _locationRepository;
    private readonly IBaseRepository<LocationOfUser> _locationOfUserRepository;

    public AccountService(
        IBaseRepository<Account> accountRepository,
        IBaseRepository<Location> locationRepository,
        IBaseRepository<LocationOfUser> locationOfUserRepository)
    {
        _accountRepository = accountRepository;
        _locationRepository = locationRepository;
        _locationOfUserRepository = locationOfUserRepository;
    }

    public async Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model)
    {
        try 
        { 
            var userExist = await (from account in _accountRepository.Select()
                                   where account.Login == model.Login
                                   select account).FirstOrDefaultAsync();
            if (userExist == null)
                return new BaseResponse<ClaimsIdentity>()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Description = "User dose not exist"
                };

            if (HashPasswordHelper.HashPassowrd(model.Password) == userExist.Password)
            {
                model.Role = model.Role;
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = Authenticate(model),
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new BaseResponse<ClaimsIdentity>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Description = "Password is wrong"
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<ValidationResult>> RegistrationAsync(RegistrationViewModel model)
    {
        try
        {
            var userExist = await (from account in _accountRepository.Select()
                                   where account.Login == model.Login || account.Email == model.Email
                                   select account).FirstOrDefaultAsync();

            if (userExist == null)
            {
                if (MaskPassword(model.Password))
                {
                    var account = new Account
                    {
                        Email = model.Email,
                        Login = model.Login,
                        Password = HashPasswordHelper.HashPassowrd(model.Password),
                        AccountConfirmed = false,
                        Role = Domain.Enums.Role.User
                    };

                    if (await _accountRepository.Add(account))
                        return new BaseResponse<ValidationResult>
                        {
                            StatusCode = HttpStatusCode.OK
                        };

                    return new BaseResponse<ValidationResult>
                    {
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }
                return new BaseResponse<ValidationResult>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new ValidationResult("Password must have numbers and only latin chars.", new List<string> { "Password" })
                };
            }
            return new BaseResponse<ValidationResult>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = new ValidationResult("Login or email have already used.", new List<string> { "Login", "Email"})
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<Account>> GetAccountByLoginAsync(string login)
    {
        try
        {
            var account = await (from user in _accountRepository.Select()
                                 where user.Login == login
                                 join location in _locationRepository.Select() 
                                 on user.Id equals location.Author.Id into locationGroup
                                 from subLocation in locationGroup.DefaultIfEmpty()
                                 //join locationOfUser in _locationOfUserRepository.Select()
                                 //on user.Id equals locationOfUser.Id into locationOfUserGroup
                                 //from subLocationOFUser in locationGroup.DefaultIfEmpty()
                                 select new Account
                                 {
                                     Id = user.Id,
                                     Login = user.Login,
                                     Email = user.Email,
                                     Password = user.Password,
                                     AccountConfirmed = user.AccountConfirmed,
                                     Role = user.Role,
                                     //LocationsCreatedByAccount = locationGroup.ToList(),
                                     //LocationsOfUser = locationOfUserGroup.ToList()
                                 }).FirstOrDefaultAsync();
            if (account == null)
                return new BaseResponse<Account>
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
            return new BaseResponse<Account>
            {
                StatusCode = HttpStatusCode.OK,
                Data = account
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<string>> ConfirmAccountAsync(int id, string hashLogin)
    {
        try
        {
            var account = await (from user in _accountRepository.Select()
                                  where user.Id == id
                                  select user).FirstOrDefaultAsync();
            if (account is null)
                return new BaseResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = "Error",
                    Description = "Account dosen't exist."
                };

            if(HashPasswordHelper.HashPassowrd(account.Login) == hashLogin)
            {
                account.AccountConfirmed = true;
                if(await _accountRepository.Update(account))
                    return new BaseResponse<string>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = "Success",
                        Description = "Email has confirmed."
                    };
            }
            return new BaseResponse<string>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = "Error",
                Description = "Incorect data."
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    private bool MaskPassword(string password)
    {
        bool onlyEn = true; // variable to check for only en key
        bool number = false; // variable to check for exist number
        for (int i = 0; i < password.Length; i++)
        {
            if (password[i] >= 'А' && password[i] <= 'Я') onlyEn = false;
            if (password[i] >= '0' && password[i] <= '9') number = true;
        }
        if (onlyEn && number)
            return true;
        return false;
    }

    private ClaimsIdentity Authenticate(LoginViewModel user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
        return new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }

    
}
