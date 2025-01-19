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

        public T1 CreateDbContext<T1>(Action<DbContextOptionsBuilder> contextConfigureAction) where T1 : DbContext
        {
            var optionBuilder = new DbContextOptionsBuilder<T>();
            contextConfigureAction.Invoke(optionBuilder);

            T context = CreateContext(optionBuilder.Options);

            return (T1)(DbContext)context;
        }

        public T CreateDbContext()
        {
            T context = CreateContext(Options);
            return context;
        }
    }
}
