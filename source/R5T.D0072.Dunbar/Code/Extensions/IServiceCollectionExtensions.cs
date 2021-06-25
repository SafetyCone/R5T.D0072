using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using R5T.Dacia;

using R5T.Dunbar.D006;
using R5T.Dunbar.D007;


namespace R5T.D0072.Dunbar
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="FunctionBasedRepositoryProvider{TRepository, TDbContext}"/> implementation of <see cref="IRepositoryProvider{TRepository}"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddFunctionBasedRepositoryProvider<TIRepository, TDbContext>(this IServiceCollection services,
            IServiceAction<IDbContextConstructorProvider<TDbContext>> dbContextConstructorProviderAction,
            Func<IDbContextConstructor<TDbContext>, Task<TIRepository>> constructor)
            where TDbContext : DbContext
        {
            services.AddSingleton<IRepositoryProvider<TIRepository>>(serviceProvider =>
                {
                    var dbContextConstructorProvider = serviceProvider.GetRequiredService<IDbContextConstructorProvider<TDbContext>>();

                    var repositoryProvider = new FunctionBasedRepositoryProvider<TIRepository, TDbContext>(
                        dbContextConstructorProvider,
                        constructor);

                    return repositoryProvider;
                })
                .Run(dbContextConstructorProviderAction)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="FunctionBasedRepositoryProvider{TRepository, TDbContext}"/> implementation of <see cref="IRepositoryProvider{TRepository}"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IRepositoryProvider<TIRepository>> AddFunctionBasedRepositoryProviderAction<TIRepository, TDbContext>(this IServiceCollection services,
            IServiceAction<IDbContextConstructorProvider<TDbContext>> dbContextConstructorProviderAction,
            Func<IDbContextConstructor<TDbContext>, Task<TIRepository>> constructor)
            where TDbContext : DbContext
        {
            var serviceAction = ServiceAction.New<IRepositoryProvider<TIRepository>>(() => services.AddFunctionBasedRepositoryProvider<TIRepository, TDbContext>(
                dbContextConstructorProviderAction,
                constructor));

            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="FunctionBasedRepositoryProvider{TRepository, TDbContext}"/> implementation of <see cref="IRepositoryProvider{TRepository}"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// Note: important to use the repository *interface* not type.
        /// </summary>
        public static IServiceAction<IRepositoryProvider<TIRepository>> AddFunctionBasedRepositoryProviderAction<TIRepository, TDbContext>(this IServiceCollection services,
            IServiceAction<IDbContextConstructorProvider<TDbContext>> dbContextConstructorProviderAction,
            Func<IDbContextConstructor<TDbContext>, TIRepository> constructor)
            where TDbContext : DbContext
        {
            return services.AddFunctionBasedRepositoryProviderAction<TIRepository, TDbContext>(
                dbContextConstructorProviderAction,
                TaskHelper.MakeAsynchronous(constructor));
        }
    }
}
