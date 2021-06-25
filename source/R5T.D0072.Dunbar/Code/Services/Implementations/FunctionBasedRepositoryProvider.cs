using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using R5T.Dunbar.D006;
using R5T.Dunbar.D007;


namespace R5T.D0072.Dunbar
{
    public class FunctionBasedRepositoryProvider<TRepository, TDbContext> : RepositoryProviderBase<TRepository, TDbContext>
        where TDbContext : DbContext
    {
        private Func<IDbContextConstructor<TDbContext>, Task<TRepository>> Constructor { get; }


        public FunctionBasedRepositoryProvider(IDbContextConstructorProvider<TDbContext> dbContextConstructorProvider,
            Func<IDbContextConstructor<TDbContext>, Task<TRepository>> constructor)
            : base(dbContextConstructorProvider)
        {
            this.Constructor = constructor;
        }

        protected override Task<TRepository> ConstructRepository(IDbContextConstructor<TDbContext> dbContextConstructor)
        {
            return this.Constructor(dbContextConstructor);
        }
    }
}
