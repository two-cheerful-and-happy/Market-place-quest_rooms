namespace Domain.Entities;

public class Photo
{
    public int Id { get; set; }
    public byte[] Value { get; set; }
    public string Address { get; set; }
    public Location LocationId { get; set;}
}
