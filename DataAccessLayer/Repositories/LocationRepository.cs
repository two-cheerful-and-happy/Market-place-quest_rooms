using Domain.Entities;
using DataAccessLayer.Entity_Framework;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class LocationRepository : IBaseRepository<Location>
{
    private readonly ApplicationDbContext _dbContext;
    public LocationRepository(ApplicationDbContext context) => _dbContext = context;

    public async Task<bool> Add(Location entity)
    {
        try
        {
            _dbContext.LocationTable.Add(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Location entity)
    {
        try
        {
            _dbContext.LocationTable.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Location> Select()
    {
        return _dbContext.LocationTable;
    }

    public async Task<bool> Update(Location entity)
    {
        try
        {
            _dbContext.LocationTable.Update(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}
