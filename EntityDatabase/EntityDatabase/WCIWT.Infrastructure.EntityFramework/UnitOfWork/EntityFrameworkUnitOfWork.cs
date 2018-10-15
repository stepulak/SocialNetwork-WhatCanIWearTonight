using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.EntityFramework.UnitOfWork
{
    public class EntityFrameworkUnitOfWork : UnitOfWorkBase
    {
        public DbContext Context { get; }

        public EntityFrameworkUnitOfWork(Func<DbContext> dbContextFactory)
        {
            Context = dbContextFactory?.Invoke() ??
                           throw new ArgumentException("Database context factory cannot be null");
        }

        protected override async Task CommitCore()
        {
            await Context.SaveChangesAsync();
        }

        public override void Dispose()
        {
            Context.Dispose();
        }
    }
}