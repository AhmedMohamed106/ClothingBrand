using System.Linq.Expressions;
using infrastructure.Data;
using Octagram.Domain.Repositories;


namespace Octagram.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    protected readonly AppDbContext _context ;
    public GenericRepository(AppDbContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>
    /// The entity with the specified ID, or null if no such entity exists.
    /// </returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <returns>
    /// An IQueryable collection of all entities of the specified type.
    /// </returns>
    public IQueryable<T> GetAllAsync()
    {
        return _context.Set<T>().AsQueryable();
    }

    /// <summary>
    /// Retrieves entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities by.</param>
    /// <returns>
    /// An IQueryable collection of entities that satisfy the predicate.
    /// </returns>
    public IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).AsQueryable();
    }

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity; 
    }

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}