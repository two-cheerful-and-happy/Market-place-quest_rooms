using Domain.Entities;

namespace Domain.ViewModels.Map;

public class OverviewLocationViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string AuthorName { get; set; }
    public string Address { get; set; }
    public Mark Mark { get; set; }
    public byte[] Photo { get; set; }
    public List<Entities.Comment> Comments { get; set; }
}
