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

        public AsyncPocoRepository(IUnitOfWorkProvider provider)
        {
            this.provider = provider;
        }

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

        public async Task<TEntity> GetAsync(Guid id, params string[] includes)
        {
            var entity = await GetAsync(id);
            var sql = Sql.Builder
                .Select("*")
                .From(entity.TableName);
            /*foreach (var include in includes)
            {
                sql = sql.LeftJoin(include).On($"{include}.Id = {entity.TableName}.Id");
            }*/
            return await Database.FirstAsync<TEntity>(sql);
        }

        public async void Update(TEntity entity)
        {
            await Database.UpdateAsync(entity);
        }
    }
}
