using Domain.ViewModels.Comment;

namespace BusinessLogic.Services;

public class CommentService : ICommentService
{
    private readonly IBaseRepository<Comment> _commentRepository;
    private readonly IBaseRepository<Location> _locationRepository;
    private readonly IBaseRepository<Account> _accountRepository;
    public CommentService(
        IBaseRepository<Comment> commentRepository,
        IBaseRepository<Location> locationRepository,
        IBaseRepository<Account> accountRepository)
    {
        _commentRepository = commentRepository;
        _locationRepository = locationRepository;
        _accountRepository = accountRepository;
    }

    public async Task<BaseResponse<Comment>> AddCommentAsync(AddCommentViewModel model)
    {
        try
        {
            var location = await _locationRepository.Select().Where(x => x.Id == model.LocationId).FirstOrDefaultAsync();
            var user = await _accountRepository.Select().Where(x => x.Login == model.Name).FirstOrDefaultAsync();
            if (user == null || location == null)
                return new BaseResponse<Comment>
                {
                    Description = "Parametters are wrong",
                    StatusCode = HttpStatusCode.BadRequest
                };
            Comment comment = new();
            comment.Account = user;
            comment.Location = location;
            comment.Mark = model.Mark;
            comment.Text = model.Comment;
            if (await _commentRepository.Add(comment))
                return new BaseResponse<Comment>()
                {
                    Data = comment,
                    StatusCode = HttpStatusCode.OK,
                    Description = "Comment was added."
                };
            return new BaseResponse<Comment>()
            {
                Description = "Server error",
                StatusCode = HttpStatusCode.InternalServerError
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
}
