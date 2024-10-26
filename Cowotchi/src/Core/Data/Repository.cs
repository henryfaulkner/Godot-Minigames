using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T>, IDisposable where T : class
{
	private AppDbContext _context;
	private bool _disposed = false; // To detect redundant calls
	
	public Repository(AppDbContext appDBContext)
	{
		_context = appDBContext;
	}

	public async Task AddAsync(T entity)
	{
		await _context.Set<T>().AddAsync(entity);
	}

	public async Task AddRangeAsync(IEnumerable<T> entities)
	{
		await _context.Set<T>().AddRangeAsync(entities);
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _context.Set<T>().ToListAsync();
	}

	public async Task<T> GetByIdAsync(int id)
	{
		return await _context.Set<T>().FindAsync(id);
	}

	public void Remove(T entity)
	{
		_context.Set<T>().Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		_context.Set<T>().RemoveRange(entities);
	}

	public async Task<bool> SaveRepositoryAsync()
	{
		var rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<bool> AnyAsync()
	{
		return await _context.Set<T>().AnyAsync();
	}
	
	// Implementation of Dispose pattern
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				// Dispose managed resources.
				_context?.Dispose();
			}
			// Free unmanaged resources (if any) here.

			_disposed = true;
		}
	}
}
