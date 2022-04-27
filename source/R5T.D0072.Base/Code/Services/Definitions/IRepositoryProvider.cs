using System;
using System.Threading.Tasks;

using R5T.T0064;


namespace R5T.D0072
{
    [ServiceDefinitionMarker]   
    public interface IRepositoryProvider<TRepository> : IServiceDefinition
    {
        Task<TRepository> GetRepository(string dataSourceName);
    }
}
