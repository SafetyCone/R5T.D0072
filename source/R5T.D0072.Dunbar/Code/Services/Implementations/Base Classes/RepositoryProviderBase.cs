using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using R5T.Dunbar.D006;
using R5T.Dunbar.D007;


namespace R5T.D0072.Dunbar
{
    public abstract class RepositoryProviderBase<TRepository, TDbContext> : IRepositoryProvider<TRepository>
        where TDbContext : DbContext
    {
        private IDbContextConstructorProvider<TDbContext> DbContextConstructorProvider { get; }


        public RepositoryProviderBase(
            IDbContextConstructorProvider<TDbContext> dbContextConstructorProvider)
        {
            this.DbContextConstructorProvider = dbContextConstructorProvider;
        }

        public async Task<TRepository> GetRepository(string dataSourceName)
        {
            // Assume that the data-source name requires no conversion to a database name.
            var databaseName = dataSourceName;

            var dbContextConstructor = await this.DbContextConstructorProvider.GetDbContextConstructor(databaseName);

            var repository = await this.ConstructRepository(dbContextConstructor);
            return repository;
        }

        protected abstract Task<TRepository> ConstructRepository(IDbContextConstructor<TDbContext> dbContextConstructor);
    }
}
