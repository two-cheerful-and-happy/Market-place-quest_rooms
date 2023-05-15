using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModels.AdditionalViewModel;

public class FilterLocationPanelViewModel
{
    public FilterLocationPanelViewModel(List<string> confirmedChoose, string confirmed, string name, string author)
    {
        confirmedChoose.Insert(0, "All");
        ConfirmedChoose = new SelectList(confirmedChoose, confirmed);
        Name = name;
        Author = author;
    }
    public string Name { get; }
    public SelectList ConfirmedChoose { get; }
    public string Author { get; } 
}
