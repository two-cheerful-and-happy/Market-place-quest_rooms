using Domain.ViewModels.AdditionalViewModel;
using Domain.ViewModels.AdminPanel;

namespace BusinessLogic.Interfaces;

public interface IAdminPanelService 
{
    Task<BaseResponse<AdminPanelAccountViewModel>> ChangeUserRoleAsync(AdminPanelAccountViewModel model, string LoginOfAdmin);
    Task<BaseResponse<AdminPanelLocationViewModel>> ConfirmLocationAsync(AdminPanelLocationViewModel model);
    Task<BaseResponse<AdminPanelAccountViewModel>> GetAccountFromCacheAsync(int id);
    Task<BaseResponse<AdminPanelLocationViewModel>> GetLoationFromCacheAsync(int id);
    Task<BaseResponse<PanelViewModel>> SetPanelViewModel(NewFilterOfAccountPanel newFilter, bool isUpdate);
    Task<BaseResponse<LocationPanelViewModel>> SetLocationPanelViewModel(NewFilterOfLocationPanel newFilter, bool isUpdate);

}
