namespace Domain.ViewModels.AdminPanel;

public class PanelViewModel
{
    public IEnumerable<AdminPanelAccountViewModel> Accounts { get; set; }
    public FilterAccountPanelViewModel FilterUserMangeViewModel { get; set; }
    public PageViewModel PageViewModel { get; set; }

    public PanelViewModel(IEnumerable<AdminPanelAccountViewModel> accounts,
        PageViewModel pageViewModel,
        FilterAccountPanelViewModel filterUserMangeViewModel)
    {
        Accounts = accounts;
        FilterUserMangeViewModel = filterUserMangeViewModel;
        PageViewModel = pageViewModel;
    }
}
