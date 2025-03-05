using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly InMemoryDbSet<T> _dbSet;

    public InMemoryRepository(InMemoryDbSet<T> dbSet)
    {
        _dbSet = dbSet;
    }

    public async Task<T> GetAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }
}