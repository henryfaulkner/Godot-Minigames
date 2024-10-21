using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
	Task<bool> SaveChanges();
}
