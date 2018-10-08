using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using WCIWT.Infrastructure.EntityFramework.UnitOfWork;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly IUnitOfWorkProvider provider;

        protected DbContext Context => ((EntityFrameworkUnitOfWork) provider.GetUnitOfWorkInstance()).Context;

        public void Create(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            Context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            var entityToUpdate = Context.Set<TEntity>().Find(entity.Id);
            Context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        }

        public void Delete(Guid id)
        {
            var entityToRemove = Context.Set<TEntity>().Find(id);
            if (entityToRemove != null)
            {
                Context.Set<TEntity>().Remove(entityToRemove);
            }
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetAsync(Guid id, params string[] includes)
        {
            DbQuery<TEntity> localContext = Context.Set<TEntity>();
            foreach (var toInclude in includes)
            {
                localContext.Include(toInclude);
            }

            return await localContext.SingleOrDefaultAsync(entity => entity.Id.Equals(id));
        }
    }
}
