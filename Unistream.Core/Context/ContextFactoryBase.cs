using Microsoft.EntityFrameworkCore;

namespace Unistream.Core.Context
{
    public abstract class ContextFactoryBase<T> : IDbContextFactory<T>
        where T : DbContext
    {
        protected DbContextOptions<T> Options;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Настройки подключения к БД</param>
        protected ContextFactoryBase(DbContextOptions<T> options)
        {
            Options = options;
        }

        public abstract T CreateContext(DbContextOptions<T> dbOptions);

        public T CreateDbContext()
        {
            T context = CreateContext(Options);
            return context;
        }
    }
}
