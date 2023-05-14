namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Mark Mark { get; set; }
    public Account Account { get; set; }
    public Location Location { get; set; }
}
