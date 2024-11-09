using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
	Task<T> GetByIdAsync(int id);
	Task<T> GetByIdIncludesAsync(int id, params Func<IQueryable<T>, IQueryable<T>>[] includes);
	Task<IEnumerable<T>> GetAllAsync();
	Task<IEnumerable<T>> GetAllIncludesAsync(params Func<IQueryable<T>, IQueryable<T>>[] includes);
	Task AddAsync(T entity);
	Task AddRangeAsync(IEnumerable<T> entities);
	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entities);
	Task<bool> SaveRepositoryAsync();
	Task<bool> AnyAsync();
	Task<List<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> queryFunc);
}
