using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories;

public interface IRepository<TEntity, TContext> where TEntity : BaseEntity where TContext : DbContext
{
    TEntity? Get(int id);
    IQueryable<TEntity> GetAll();
    List<TEntity>? GetAll(Expression<Func<TEntity, bool>> predicate);
    void Delete(int id);
    void Delete(TEntity entity);
    void Update(TEntity entity);
    void Add(TEntity entity);
    int SaveChanges();
}
