using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.EntityFramework.UnitOfWork
{
    class EntityFrameworkUnitOfWorkProvider : UnitOfWorkProviderBase
    {
        private readonly Func<DbContext> dbContextFactory;

        public EntityFrameworkUnitOfWorkProvider(Func<DbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public override IUnitOfWork Create()
        {
            UnitOfWorkLocalInstance.Value = new EntityFrameworkUnitOfWork(dbContextFactory);
            return UnitOfWorkLocalInstance.Value;
        }
    }
}
