namespace Domain.Entities;

public class Location 
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool LocationConfirmed { get; set; }
    public byte[] Photo { get; set; }
    public Account Author { get; set; } 
    public ICollection<Comment> CommentsOfLocation { get; set; }
}
