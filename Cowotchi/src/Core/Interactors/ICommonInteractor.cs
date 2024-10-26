using System;
using System.Threading.Tasks;

public interface ICommonInteractor
{
    public Task InitDatabaseIfRequired();
}