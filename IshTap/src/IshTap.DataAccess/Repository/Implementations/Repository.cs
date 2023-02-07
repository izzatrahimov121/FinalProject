using IshTap.Core.Interfaces;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IshTap.DataAccess.Repository.Implementations;

public class Repository<T> : IRepository<T> where T : class, IEntity, new()
{
    private readonly AppDbContexts _contexts;

    public Repository(AppDbContexts contexts)
    {
        _contexts = contexts;
    }

    public DbSet<T> Table => _contexts.Set<T>();

    public IQueryable<T> FindAll() => Table.AsQueryable().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool IsTracking = true)
    {
        if (IsTracking) { return Table.Where(expression); }
        return Table.Where(expression).AsNoTracking();
    }

    public async Task<T?> FindByIdAsync(int id) => await Table.FindAsync(id);


    public async Task CreateAsync(T entity) => await Table.AddAsync(entity);


    public void Delete(T entity) => Table.Remove(entity);

    public void Update(T entity)
    {
        Table.Update(entity);
    }
    public async Task SaveAsync() => await _contexts.SaveChangesAsync();
}
