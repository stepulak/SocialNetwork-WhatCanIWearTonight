using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.UnitOfWork;
using WCIWT.Infrastructure.EntityFramework.UnitOfWork;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.EntityFramework;
using WCIWT.Infrastructure.Query;
using EntityDatabase.Migrations;

namespace EntityDatabase.Config
{
    public class EntityFrameworkInstaller : IWindsorInstaller
    {
        internal const string ConnectionString = "Data source=(localdb)\\mssqllocaldb;Database=WCIWTDatabaseSample;Trusted_Connection=True;MultipleActiveResultSets=true";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<DbContext>>()
                   .Instance(() => new WCIWTDbContext())
                    .LifestyleTransient(),
                Component.For<IUnitOfWorkProvider>()
                    .ImplementedBy<EntityFrameworkUnitOfWorkProvider>()
                    .LifestyleSingleton(),
                Component.For(typeof(IRepository<>))
                    .ImplementedBy(typeof(EntityFrameworkRepository<>))
                    .LifestyleTransient(),
                Component.For(typeof(IQuery<>))
                    .ImplementedBy(typeof(EntityFrameworkQuery<>))
                    .LifestyleTransient()
                );
        }
    }
}
