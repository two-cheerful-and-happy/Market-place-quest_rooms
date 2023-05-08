using Azure;
using DataAccessLayer.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.ViewModels.AdditionalViewModel;
using Domain.ViewModels.AdminPanel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System.Collections;

namespace BusinessLogic.Services;

public class AdminPanelService : IAdminPanelService
{
    private readonly IBaseRepository<Account> _accountRepository;
    
    private readonly IMemoryCache _memoryCache;
    private const string _listKey = "AccountsListKey";
    private const string _requestsOnChangingRoleListKey = "RequestsOnChangingRoleListKey";
    private List<string> _rolesOfFilter = new();

    public AdminPanelService(
        IBaseRepository<Account> accountRepository,
        IMemoryCache memoryCache)
    {
        _accountRepository = accountRepository;
        _memoryCache = memoryCache;

        _rolesOfFilter.Add(Role.User.ToString());
        _rolesOfFilter.Add(Role.OwnerOfRooms.ToString());
        _rolesOfFilter.Add(Role.Manager.ToString());
        _rolesOfFilter.Add(Role.Admin.ToString());
    }

    

    public async Task<BaseResponse<PanelViewModel>> SetPanelViewModel(NewFilterOfAccountPanel newFilter, bool isUpdate)
    {
        try
        {
            PanelViewModel panelViewModel;
            IEnumerable<AdminPanelAccountViewModel> source; 
            if (isUpdate)
                source = UpdateAccountsInCache();
            else
                source = GetAccountsFromCache();

            int pageSize = 15;
            if (!string.IsNullOrEmpty(newFilter.Role))
                source = source.Where(x => x.Role.ToString() == newFilter.Role);
            if(!string.IsNullOrEmpty(newFilter.Login))
                source = source.Where(x => x.Login!.Contains(newFilter.Login));

            var count = source.Count();
            var items = source.Skip((newFilter.Page - 1) * pageSize).Take(pageSize).ToList();

            var pageViewModel = new PageViewModel(count, newFilter.Page, pageSize);

            panelViewModel = new(source, pageViewModel,
                new FilterAccountPanelViewModel(_rolesOfFilter, newFilter.Role, newFilter.Login));

            return new BaseResponse<PanelViewModel>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = panelViewModel
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<AdminPanelAccountViewModel>> ChangeUserRoleAsync(AdminPanelAccountViewModel model, string loginOfAdmin)
    {
        try
        {
            var admin = await _accountRepository.Select().Where(x => x.Login == loginOfAdmin).FirstOrDefaultAsync();
            var user = await _accountRepository.Select().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (admin == null)
                return new BaseResponse<AdminPanelAccountViewModel>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Description = "Server error"
                };
            switch (admin.Role)
            {
                case Role.Manager:
                    if (model.Role == Role.Admin || user.Role == Role.Admin || model.Role == Role.Manager || user.Role == Role.Manager)
                    {
                        return new BaseResponse<AdminPanelAccountViewModel>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Description = "you don't have enough authority"
                        };
                    }
                    break;
                case Role.Admin:
                    if (model.Role == Role.Admin || user.Role == Role.Admin)
                    {
                        return new BaseResponse<AdminPanelAccountViewModel>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Description = "you don't have enough authority"
                        };
                    }
                    break;
                default:
                    break;
            }
            
            
            user.Role = model.Role;
            if (await _accountRepository.Update(user))
                return new BaseResponse<AdminPanelAccountViewModel>
                {
                    StatusCode = HttpStatusCode.OK,
                };
            return new BaseResponse<AdminPanelAccountViewModel>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Description = "Server error"
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task SetAccountsToCache()
    {
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));
        var accounts = await _accountRepository.Select().ToListAsync();

        _memoryCache.Set(_listKey, accounts, cacheOptions);
    }

    public async Task<BaseResponse<AdminPanelAccountViewModel>> GetAccountFromCacheAsync(int id)
    {
        try
        {
            AdminPanelAccountViewModel account = new();
            _memoryCache.TryGetValue(_listKey, out List<AdminPanelAccountViewModel>? list);

            if (list == null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                var accountsFromCache = _memoryCache.GetOrCreate(_listKey, entry =>
                {
                    entry.SetOptions(cacheOptions);
                    return (from account in _accountRepository.Select()
                            select new AdminPanelAccountViewModel
                            {
                                Id = account.Id,
                                Email = account.Email,
                                Login = account.Login,
                                Role = account.Role,
                                AccountConfirmed = account.AccountConfirmed
                            }).ToList();
                });
            }
            else
            {
                account = list.Find(x => x.Id == id);
            }

            return new BaseResponse<AdminPanelAccountViewModel>()
            {
                Data = account,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    private List<AdminPanelAccountViewModel> GetAccountsFromCache()
    {
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        var accountsFromCache = _memoryCache.GetOrCreate(_listKey, entry =>
        {
            entry.SetOptions(cacheOptions);
            return (from account in _accountRepository.Select()
                    select new AdminPanelAccountViewModel
                    {
                        Id = account.Id,
                        Email = account.Email,
                        Login = account.Login,
                        Role = account.Role,
                        AccountConfirmed = account.AccountConfirmed
                    }).ToList();
        });
        return accountsFromCache;
    }

    private List<AdminPanelAccountViewModel> UpdateAccountsInCache()
    {
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        _memoryCache.Remove(_listKey);
        var accountsFromCache = _memoryCache.GetOrCreate(_listKey, entry =>
        {
            entry.SetOptions(cacheOptions);
            return (from account in _accountRepository.Select()
                    select new AdminPanelAccountViewModel
                    {
                        Id = account.Id,
                        Email = account.Email,
                        Login = account.Login,
                        Role = account.Role,
                        AccountConfirmed = account.AccountConfirmed
                    }).ToList();
        });
        return accountsFromCache;
    }

    public Task<BaseResponse<PanelViewModel>> UpdatePanelViewModel(NewFilterOfAccountPanel newFilter)
    {
        throw new NotImplementedException();
    }
}
