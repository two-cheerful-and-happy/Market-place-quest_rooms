using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Interfaces;

public interface IAccountService
{
    public Task<BaseResponse<ValidationResult>> RegistrationAsync(RegistrationViewModel model);
    public Task<BaseResponse<Account>> GetAccountByLoginAsync(string login);
    public Task<BaseResponse<string>> ConfirmAccountAsync(int id, string hashLogin);
    public Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model);
    public Task<List<Account>> SelectAccountsAsync();
    public Task<BaseResponse<string>> CreateNewRequestOnChangingRole(CreateRequestToChangingRoleViewModel model);
}
