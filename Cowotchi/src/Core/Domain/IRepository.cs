using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
	Task<T> GetByIdAsync(int id);
	Task<IEnumerable<T>> GetAllAsync();
	Task AddAsync(T entity);
	Task AddRangeAsync(IEnumerable<T> entities);
	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entities);
	Task<bool> SaveRepositoryAsync();
	Task<bool> AnyAsync();
}
