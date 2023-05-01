using Domain.Entities;
using DataAccessLayer.Entity_Framework;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class CommentRepository : IBaseRepository<Comment>
{
    private readonly ApplicationDbContext _dbContext;
    public CommentRepository(ApplicationDbContext context) => _dbContext = context;

    public async Task<bool> Add(Comment entity)
    {
        try
        {
            _dbContext.CommentTable.Add(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Comment entity)
    {
        try
        {
            _dbContext.CommentTable.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Comment> Select()
    {
        return _dbContext.CommentTable;
    }

    public async Task<bool> Update(Comment entity)
    {
        try
        {
            _dbContext.CommentTable.Update(entity);
            _dbContext.SaveChanges();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}
