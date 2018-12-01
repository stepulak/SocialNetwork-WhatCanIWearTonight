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

namespace EntityDatabase.Config
{
    public class EntityFrameworkInstaller : IWindsorInstaller
    {
        internal const string ConnectionString = "Data source=(localdb)\\mssqllocaldb;Database=WCIWTDatabaseSample;Trusted_Connection=True;MultipleActiveResultSets=true";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<DbContext>>()
                   .Instance(() => CreateContext())
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

        public WCIWTDbContext CreateContext()
        {
            var context = new WCIWTDbContext();
            if (context.Users.Where(u => u.Username == "Miss Fortune").Count() == 0)
            {
                context.Users.Add(new User
                {
                    Id = Guid.Parse("25d1461d-41db-4a5a-8996-dd0fcf7f5f04"),
                    Username = "Miss Fortune",
                    Email = "missfortune@gold.com",
                    Birthdate = new DateTime(1995, 12, 14, 13, 26, 52),
                    Gender = Gender.Female,
                    PasswordHash = "0xBEEF",
                    PasswordSalt = "0xBAD007",
                    IsAdmin = false
                });
            }
            context.SaveChanges();
            return context;
        }
    }
}
