namespace DataAccessLayer.Repositories;

public class LocationOfUserRepository : IBaseRepository<LocationOfUser>
{
    private readonly ApplicationDbContext _dbContext;
    public LocationOfUserRepository(ApplicationDbContext context) => _dbContext = context;

    public async Task<bool> Add(LocationOfUser entity)
    {
        try
        {
            _dbContext.LocationOfUserTable.Add(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(LocationOfUser entity)
    {
        try
        {
            _dbContext.LocationOfUserTable.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<LocationOfUser> Select()
    {
        return _dbContext.LocationOfUserTable;
    }

    public async Task<bool> Update(LocationOfUser entity)
    {
        try
        {
            _dbContext.LocationOfUserTable.Update(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}
