using Domain.DTO;
using Domain.Entities;
using System.ComponentModel;

namespace BusinessLogic.Services;

public class AccountService : IAccountService
{
    private readonly IBaseRepository<Account> _accountRepository;
    private readonly IBaseRepository<Location> _locationRepository;
    

    public AccountService(
        IBaseRepository<Account> accountRepository,
        IBaseRepository<Location> locationRepository
        )
       
    {
        _accountRepository = accountRepository;
        _locationRepository = locationRepository;
       
    }

    

    public async Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model)
    {
        try 
        { 
            var userExist = await (from account in _accountRepository.Select()
                                   where account.Login == model.Login || account.Email == model.Login && account.AccountConfirmed == true
                                   select account).FirstOrDefaultAsync();
            if (userExist == null)
                return new BaseResponse<ClaimsIdentity>()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Description = "User dose not exist, or account dosen't confirmed"
                };

            if(!userExist.AccountConfirmed)
                return new BaseResponse<ClaimsIdentity>()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Description = "Account dosn't confirmd"
                };

            if (HashPasswordHelper.HashPassowrd(model.Password) == userExist.Password)
            {
                model.Role = userExist.Role;
                model.Login = userExist.Login;
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = Authenticate(model),
                    StatusCode = HttpStatusCode.OK,
                    Description = userExist.Role.ToString()
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

    public async Task<BaseResponse<ValidationResult>> ChangePasswordAsync(ChangingAccountPasswordViewModel model)
    {
        try
        {
            var user = await _accountRepository.Select().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if(user.Password == HashPasswordHelper.HashPassowrd(model.Password))
                return new BaseResponse<ValidationResult>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new ValidationResult("The password is the same.", new List<string> { "Password" })
                };
            if (MaskPassword(model.Password))
            {
                user.Password = HashPasswordHelper.HashPassowrd(model.Password);
                if (await _accountRepository.Update(user))
                    return new BaseResponse<ValidationResult>()
                    {
                        StatusCode = HttpStatusCode.OK
                    };
            }
            return new BaseResponse<ValidationResult>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = new ValidationResult("The password must not contain Cyrillic characters.", new List<string> { "Password" })
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
                        Role = model.Role
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
                    Data = new ValidationResult("Password must have numbers and only latin chars.", new List<string> { "Registration.Password" })
                };
            }
            return new BaseResponse<ValidationResult>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = new ValidationResult("Login or email have already used.", new List<string> { "Registration.Login", "Registration.Email" })
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
            var account = await _accountRepository.Select()
                .Include(x => x.CommentsCreatedByAccount)
                .Include(x => x.LocationsCreatedByAccount)
                .FirstOrDefaultAsync();
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

    public async Task<BaseResponse<Account>> GetAccountByEmailAsync(string email)
    {
        try
        {
            var user = await _accountRepository.Select().Where(x => x.Email == email).FirstOrDefaultAsync();
            if(user == null)
                return new BaseResponse<Account>
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
            return new BaseResponse<Account>
            {
                StatusCode = HttpStatusCode.OK,
                Data = user
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public Task<BaseResponse<string>> ChangingAccountPassword(int id)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<string>> ChangingAccountEmail(int id)
    {
        throw new NotImplementedException();
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

    public async Task<List<Account>> SelectAccountsAsync()
    {
        try
        {
            var result = await _accountRepository.Select().ToListAsync();
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task<BaseResponse<Account>> GetAccountByIdAsync(int id)
    {
        try
        {
            var user = await _accountRepository.Select().Where(x => x.Id == id).FirstOrDefaultAsync();
            return new BaseResponse<Account>
            {
                StatusCode = HttpStatusCode.OK,
                Data = user
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public AccountCookieData GetCookieAccountByLogin(string login)
    {
        try
        {
            var user = (from p in _accountRepository.Select()
                        where p.Login == login
                        select new AccountCookieData
                        {
                            Login = p.Login,
                            Address = p.Address,
                            Birthday = p.Birthday,
                            Email = p.Email,
                            PhoneNumber = p.PhoneNumber,
                            Role = p.Role
                        }).FirstOrDefault();
            return user;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<AccountCookieData>> ChangeEmailAsync(ChangeEmailViewModel model)
    {
        try
        {
            var user = await _accountRepository.Select().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            var existEmail = await _accountRepository.Select().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            if (user is null)
                return new BaseResponse<AccountCookieData> { StatusCode = HttpStatusCode.BadRequest, Description = "User dose not exist" };
            if (existEmail != null)
                return new BaseResponse<AccountCookieData> { StatusCode = HttpStatusCode.BadRequest, Description = "Email have already used" };
            if (HashPasswordHelper.HashPassowrd(user.Login) == model.Token)
            {
                user.Email = model.Email;
                user.AccountConfirmed = false;
                if (await _accountRepository.Update(user))
                {
                    var result = new AccountCookieData
                    {
                        Address = user.Address,
                        Birthday = user.Birthday,
                        Email = user.Email,
                        Login = user.Login,
                        PhoneNumber = user.PhoneNumber,
                        Role = user.Role
                    };
                    return new BaseResponse<AccountCookieData>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = result
                    };
                }
            }
            return new BaseResponse<AccountCookieData> { StatusCode = HttpStatusCode.BadRequest, Description = "Token is wrong" };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<BaseResponse<AccountCookieData>> ChangeLoginAsync(ChangeLoginViewModel model)
    {
        try
        {
            if(model.Token == model.Login)
                return new BaseResponse<AccountCookieData>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "Login is same."
                };
            var existEmail = await _accountRepository.Select().Where(x => x.Login == model.Login).FirstOrDefaultAsync();
            if (existEmail != null)
                return new BaseResponse<AccountCookieData>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "Login have already used."
                };
            var user = await _accountRepository.Select().Where(x => x.Login == model.Token).FirstOrDefaultAsync();
            user.Login = model.Login;
            await _accountRepository.Update(user);
            return new BaseResponse<AccountCookieData>
            {
                Data = new AccountCookieData
                {
                    Address = user.Address,
                    Birthday = user.Birthday,
                    Email = user.Email,
                    Login = user.Login,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                },
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<ValidationResult>> ChangePasswordAsync(ChangePasswordViewModel model)
    {
        try
        {
            var user = await _accountRepository.Select().Where(x => x.Login == model.Token).FirstOrDefaultAsync();
            if (HashPasswordHelper.HashPassowrd(model.CurrentPassword) != user.Password)
                return new BaseResponse<ValidationResult>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = new ValidationResult("The password is wrong.", new List<string> { "CurrentPassword" })
                };
            if(model.NewPassword == model.NewPasswordConfirm && HashPasswordHelper.HashPassowrd(model.NewPassword) != user.Password)
            {
                if(ChangePassword(ref user, model.NewPassword))
                    return new BaseResponse<ValidationResult>
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
            }
            
            return new BaseResponse<ValidationResult>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = new ValidationResult("The password must not contain Cyrillic characters. Or new password is same", new List<string> { "NewPassword" })
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

    private bool ChangePassword(ref Account account,string password)
    {
        if (MaskPassword(password))
        {
            account.Password = password;
            return true;
        }

        return false;
    }
}
