using System.Linq.Expressions;
using LibraryManagement.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Web.Data.Repositories;

public class Repository<T, TId>(AppDbContext context) : IRepository<T, TId>
    where T : BaseEntity<TId>
    where TId : struct
{
    public DbSet<T> Table => context.Set<T>();

    public IQueryable<T> GetAll()
    {
        return Table.AsQueryable().AsNoTracking();
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return Table.Where(predicate).AsNoTracking();
    }

    public ValueTask<T?> GetByIdAsync(TId id)
    {
        return Table.FindAsync(id);
    }

    public async ValueTask<bool> AddAsync(T entity)
    {
        var entityEntry = await Table.AddAsync(entity);
        return entityEntry.State == EntityState.Added;
    }

    public bool Update(T entity)
    {
        var entityEntry = Table.Update(entity);
        return entityEntry.State == EntityState.Modified;
    }

    public bool Delete(T entity)
    {
        var entityEntry = Table.Remove(entity);
        return entityEntry.State == EntityState.Deleted;
    }

    public async Task<bool> DeleteByIdAsync(TId id)
    {
        var entity = await Table.FindAsync(id);
        return entity != null && Delete(entity);
    }
    public Task<bool> ExistsAsync(TId id)
    {
        return Table.AnyAsync(e => e.Id.Equals(id));
    }
}