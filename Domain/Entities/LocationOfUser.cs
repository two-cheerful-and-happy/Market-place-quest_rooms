namespace Domain.Entities;

public class LocationOfUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public ICollection<Account> LocationsOfUser { get; set;}
}
