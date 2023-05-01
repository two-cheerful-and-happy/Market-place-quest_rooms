namespace DataAccessLayer.Repositories;

public class RequestOnChangingRoleRepository : IBaseRepository<RequestOnChangingRole>
{
    private readonly ApplicationDbContext _dbContext;
    public RequestOnChangingRoleRepository(ApplicationDbContext context) => _dbContext = context;

    public async Task<bool> Add(RequestOnChangingRole entity)
    {
        try
        {
            _dbContext.RequestOnChangingRoleTable.Add(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(RequestOnChangingRole entity)
    {
        try
        {
            _dbContext.RequestOnChangingRoleTable.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<RequestOnChangingRole> Select()
    {
        return _dbContext.RequestOnChangingRoleTable;
    }

    public async Task<bool> Update(RequestOnChangingRole entity)
    {
        try
        {
            _dbContext.RequestOnChangingRoleTable.Update(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}
