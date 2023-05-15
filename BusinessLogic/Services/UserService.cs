namespace BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IBaseRepository<Account> _accountRepository;
    private readonly IBaseRepository<Comment> _commentRepository;

    public UserService(
        IBaseRepository<Account> accountRepository, 
        IBaseRepository<Comment> commentRepository)
    {
        _accountRepository = accountRepository;
        _commentRepository = commentRepository;
    }


}
