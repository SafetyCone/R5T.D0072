using System;
using System.Threading.Tasks;


namespace R5T.D0072
{
    public interface IRepositoryProvider<TRepository>
    {
        Task<TRepository> GetRepository(string dataSourceName);
    }
}
