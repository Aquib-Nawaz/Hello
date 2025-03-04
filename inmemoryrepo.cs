using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity<int>
{
    private readonly List<T> _entities = new();

    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult(_entities.AsQueryable().FirstOrDefault(predicate));
    }

    public Task<T> InsertAsync(T entity)
    {
        entity.Id = _entities.Count + 1; // Auto-increment ID for testing
        _entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        var existing = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (existing != null)
        {
            _entities.Remove(existing);
        }
        _entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(T entity)
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var entity = _entities.FirstOrDefault(e => e.Id == id);
        if (entity != null)
        {
            _entities.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public Task<T> GetAsync(int id)
    {
        return Task.FromResult(_entities.FirstOrDefault(e => e.Id == id));
    }

    public Task<List<T>> GetAllListAsync()
    {
        return Task.FromResult(_entities.ToList());
    }

    public Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult(_entities.AsQueryable().Where(predicate).ToList());
    }

    public Task<long> CountAsync()
    {
        return Task.FromResult((long)_entities.Count);
    }

    public Task<long> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult((long)_entities.AsQueryable().Count(predicate));
    }
}