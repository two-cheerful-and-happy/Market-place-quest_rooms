using Domain.Entities;
using DataAccessLayer.Entity_Framework;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class PhotoRepository : IBaseRepository<Photo>
{
    private readonly ApplicationDbContext _dbContext;
    public PhotoRepository(ApplicationDbContext context) => _dbContext = context;

    public async Task<bool> Add(Photo entity)
    {
        try
        {
            _dbContext.PhotoTable.Add(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Photo entity)
    {
        try
        {
            _dbContext.PhotoTable.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Photo> Select()
    {
        return _dbContext.PhotoTable;
    }

    public async Task<bool> Update(Photo entity)
    {
        try
        {
            _dbContext.PhotoTable.Update(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}
