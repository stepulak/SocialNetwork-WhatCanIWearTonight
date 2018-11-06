using AutoMapper;
using BusinessLayer.Facades.Common;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EntityDatabase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Config
{
    public class BusinessLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            new EntityFrameworkInstaller().Install(container, store);

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn(typeof(QueryObjectBase<,,,>))
                    .WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromThisAssembly()
                    .BasedOn<ServiceBase>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Classes.FromThisAssembly()
                    .BasedOn<FacadeBase>()
                    .LifestyleTransient(),
                Component.For<IMapper>()
                .Instance(new Mapper(new MapperConfiguration(MappingConfig.ConfigureMapping)))
                .LifestyleSingleton()
                );
        }
    }
}
