using Domain.Enums;

namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Mark Mark { get; set; }
    public ICollection<Account> CommentsOfAccount { get; set; }
    public ICollection<Location> CommentsOfLocation { get; set; }
}
