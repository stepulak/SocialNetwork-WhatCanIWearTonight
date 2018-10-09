using AsyncPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.AsyncPoco.UnitOfWork
{
    public class AsyncPocoUnitOfWorkProvider : UnitOfWorkProviderBase
    {
        private readonly Func<Database> databaseCreator;

        public AsyncPocoUnitOfWorkProvider(Func<Database> contextCreator)
        {
            this.databaseCreator = databaseCreator ?? throw new ArgumentNullException("Database creator is null");
        }

        public override IUnitOfWork Create()
        {
            UnitOfWorkLocalInstance.Value = new AsyncPocoUnitOfWork(databaseCreator);
            return UnitOfWorkLocalInstance.Value;
        }
    }
}
