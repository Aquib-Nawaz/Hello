using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class InMemoryDbSet<T> : DbSet<T> where T : class
{
    private readonly List<T> _items = new List<T>();

    // Mimic FindAsync method using a simple ID lookup (assuming "Id" is the key)
    public override ValueTask<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        var property = typeof(T).GetProperty("Id"); // Assuming "Id" is the key
        var id = (int)keyValues[0];
        var entity = _items.FirstOrDefault(item => (int)property.GetValue(item) == id);
        return new ValueTask<T>(entity);
    }

    // Add new item to the in-memory list
    public override async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _items.Add(entity);
        await Task.CompletedTask;
    }

    // Update existing item in the list
    public override void Update(T entity)
    {
        var property = typeof(T).GetProperty("Id");
        var id = (int)property.GetValue(entity);
        var index = _items.FindIndex(item => (int)property.GetValue(item) == id);
        if (index != -1)
        {
            _items[index] = entity;
        }
    }

    // Remove item from the list
    public override void Remove(T entity)
    {
        _items.Remove(entity);
    }

    // Query the in-memory list
    public override IQueryable<T> AsQueryable()
    {
        return _items.AsQueryable();
    }
}