using AsyncPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.AsyncPoco.UnitOfWork;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.AsyncPoco
{
    public class AsyncPocoRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly IUnitOfWorkProvider provider;

        protected Database Database => ((AsyncPocoUnitOfWork)provider.GetUnitOfWorkInstance()).Database;

        public async void Create(TEntity entity)
        {
            entity.Id = new Guid();
            await Database.SaveAsync(entity);
        }

        public async void Delete(Guid id)
        {
            var entity = GetAsync(id).Result;
            if (entity != null)
            {
                await Database.DeleteAsync(entity);
            }
        }

        public Task<TEntity> GetAsync(Guid id)
        {
            return Database.SingleOrDefaultAsync<TEntity>(id);
        }

        public Task<TEntity> GetAsync(Guid id, params string[] includes)
        {
            Database.FetchAsync()
        }

        public async void Update(TEntity entity)
        {
            await Database.UpdateAsync(entity);
        }
    }
}
