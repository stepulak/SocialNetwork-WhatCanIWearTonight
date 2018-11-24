using System.Web.Mvc;
using BusinessLayer.Config;
using BusinessLayer.Facades;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace PresentationLayerMVC.Windsor
{
    public class PresentationLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {            
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IController>()
                    .LifestyleTransient()
            );
        }
    }
}