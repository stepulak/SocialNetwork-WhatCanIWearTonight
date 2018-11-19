using System.Web.Http.Controllers;

namespace WebApiLayer.Windsor
{
    public class WebApiInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .LifestylePerWebRequest()
            );
        }
    }
}