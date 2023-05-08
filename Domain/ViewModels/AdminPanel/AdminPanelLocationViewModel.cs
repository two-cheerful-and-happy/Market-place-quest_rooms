namespace Domain.ViewModels.AdminPanel;

public class AdminPanelLocationViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string AuthorName { get; set; }
    public int AuthorId { get; set; }
    public bool LocationConfirmed { get; set; }
}
