namespace Domain.ViewModels.AdminPanel;

public class LocationPanelViewModel
{
    public IEnumerable<AdminPanelLocationViewModel> Locations { get; set; }
    public FilterLocationPanelViewModel FilterLocationPanelViewModel { get; set; }
    public PageViewModel PageViewModel { get; set; }

    public LocationPanelViewModel(IEnumerable<AdminPanelLocationViewModel> accounts,
        PageViewModel pageViewModel,
        FilterLocationPanelViewModel filterLocationPanelViewModel)
    {
        Locations = accounts;
        FilterLocationPanelViewModel = filterLocationPanelViewModel;
        PageViewModel = pageViewModel;
    }
}
