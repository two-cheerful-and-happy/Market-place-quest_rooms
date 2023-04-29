namespace DataAccessLayer.Repositories;

public class AccountRepository : IBaseRepository<Account>
{
    private readonly ApplicationDbContext _dbContext;
    public AccountRepository(ApplicationDbContext context) => _dbContext = context;

    public async Task<bool> Add(Account entity)
    {
        try
        {
            _dbContext.AccountTable.Add(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Account entity)
    {
        try
        {
            _dbContext.AccountTable.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Account> Select()
    {
        return _dbContext.AccountTable;
    }

    public async Task<bool> Update(Account entity)
    {
        try
        {
            _dbContext.AccountTable.Update(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}
