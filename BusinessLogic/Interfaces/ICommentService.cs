using Domain.ViewModels.Comment;

namespace BusinessLogic.Interfaces;

public interface ICommentService
{
    Task<BaseResponse<Comment>> AddCommentAsync(AddCommentViewModel model);
}
