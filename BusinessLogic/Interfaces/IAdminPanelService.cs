using Domain.ViewModels.AdditionalViewModel;
using Domain.ViewModels.AdminPanel;

namespace BusinessLogic.Interfaces;

public interface IAdminPanelService 
{
    Task<BaseResponse<AdminPanelAccountViewModel>> ChangeUserRoleAsync(AdminPanelAccountViewModel model, string LoginOfAdmin);
    Task<BaseResponse<AdminPanelAccountViewModel>> GetAccountFromCacheAsync(int id);
    Task<BaseResponse<PanelViewModel>> SetPanelViewModel(NewFilterOfAccountPanel newFilter);
    Task<BaseResponse<RequestsOnChangingRoleViewModel>> GetRequestsOnChangingRoleAsync();
    Task SetAccountsToCache();

}
