using AsyncPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.AsyncPoco.UnitOfWork
{
    public class AsyncPocoUnitOfWork : UnitOfWorkBase
    {
        public Database Database { get; }

        public AsyncPocoUnitOfWork(Func<Database> databaseCreator)
        {
            Database = databaseCreator?.Invoke() ?? throw new ArgumentNullException("Database creator is null");
        }
        
        protected override async Task CommitCore()
        {
            await Transaction.BeginAsync(Database);
        }

        public override void Dispose() => Database.Dispose();
    }
}
