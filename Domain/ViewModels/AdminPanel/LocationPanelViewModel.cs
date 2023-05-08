
using Domain.Entities;

namespace Domain.ViewModels.AdminPanel;

public class LocationPanelViewModel
{
    public IEnumerable<AdminPanelLocationViewModel> Locations { get; set; }
    public FilterAccountPanelViewModel FilterUserMangeViewModel { get; set; }
    public PageViewModel PageViewModel { get; set; }

    public LocationPanelViewModel(IEnumerable<AdminPanelLocationViewModel> accounts,
        PageViewModel pageViewModel,
        FilterAccountPanelViewModel filterUserMangeViewModel)
    {
        Locations = accounts;
        FilterUserMangeViewModel = filterUserMangeViewModel;
        PageViewModel = pageViewModel;
    }
}
