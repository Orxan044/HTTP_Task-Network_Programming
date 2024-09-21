using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class Repository<TEntity, TContext> : IRepository<TEntity, TContext> where TEntity : BaseEntity where TContext : DbContext
{
    private readonly TContext _context;

    public Repository(TContext context)
    {
        _context = context;
    }

    DbSet<TEntity> Set => _context.Set<TEntity>();

    public TEntity? Get(int id)
    {
        return Set.FirstOrDefault(x => x.Id == id);
    }

    public void Add(TEntity entity)
    {
        Set.Add(entity);
    }
    public void Delete(int id)
    {
        var entity = Set.FirstOrDefault(x => x.Id == id);
        if (entity is not null)
            Set.Remove(entity);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return Set;
    }

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
    {
        return [.. Set.Where(predicate)];
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        Set.Update(entity); 
    }

}
