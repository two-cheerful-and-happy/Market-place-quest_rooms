using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.AdditionalViewModel;

public class FilterAccountPanelViewModel
{
    public FilterAccountPanelViewModel(List<string> roles, string role, string login)
    {
        roles.Insert(0, "All");
        Roles = new SelectList(roles, role);
        SelectedLogin = login;
    }
    public SelectList Roles { get; } // List roles
    public string SelectedLogin { get; } // Login entered
}
    