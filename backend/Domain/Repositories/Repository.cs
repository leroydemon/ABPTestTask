using DbLevel.Interfaces;
using Domain.Data;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DbLevel
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _context;

        // Constructor that initializes the repository with the application database context
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves all entities of type T from the database
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        // Retrieves a list of entities based on the provided specification
        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
        {
            // Start with the base query for the entity set
            IQueryable<T> query = _context.Set<T>();

            // Apply criteria filters from the specification
            query = spec.Criterias.Aggregate(query, (current, criteria) => current.Where(criteria));

            // Apply ordering if specified
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Apply grouping if specified
            if (spec.GroupBy != null)
            {
                query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            // Include related entities based on the specification
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            // Apply pagination if enabled
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Execute the query and return the result as a list
            return await query.ToListAsync();
        }

        // Adds a new entity to the database
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        // Deletes the specified entity from the database
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Retrieves an entity by its unique identifier
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Updates an existing entity in the database
        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        // Saves all changes made in the context to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}

