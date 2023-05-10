using Domain.DTO;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Interfaces;

public interface IAccountService
{
    public Task<BaseResponse<ValidationResult>> RegistrationAsync(RegistrationViewModel model);
    public Task<BaseResponse<Account>> GetAccountByLoginAsync(string login);
    public Task<BaseResponse<Account>> GetAccountByEmailAsync(string email);
    public Task<BaseResponse<Account>> GetAccountByIdAsync(int id);
    public Task<BaseResponse<ValidationResult>> ChangePasswordAsync(ChangingAccountPasswordViewModel model);
    public Task<BaseResponse<string>> ChangingAccountEmail(int id);
    public Task<BaseResponse<string>> ConfirmAccountAsync(int id, string hashLogin);
    public Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model);
    public Task<List<Account>> SelectAccountsAsync();
    public AccountCookieData GetCookieAccountByLogin(string login);
    public Task<BaseResponse<AccountCookieData>> ChangeEmailAsync(ChangeEmailViewModel model);
    public Task<BaseResponse<AccountCookieData>> ChangeLoginAsync(ChangeLoginViewModel model);
    public Task<BaseResponse<ValidationResult>> ChangePasswordAsync(ChangePasswordViewModel model);
}
