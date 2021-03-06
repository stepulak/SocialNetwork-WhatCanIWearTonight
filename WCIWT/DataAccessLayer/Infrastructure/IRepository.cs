﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WCIWT.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        Task<TEntity> GetAsync(Guid id);

        Task<TEntity> GetAsync(Guid id, params string[] includes);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(Guid id);

    }
}
