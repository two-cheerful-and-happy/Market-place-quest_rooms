namespace Domain.ViewModels.Comment;

public class AddCommentViewModel
{
    public int LocationId { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public Mark Mark { get; set; }
}
