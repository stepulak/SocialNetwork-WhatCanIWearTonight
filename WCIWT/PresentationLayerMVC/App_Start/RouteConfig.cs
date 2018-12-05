using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PresentationLayerMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "Default",
                "{action}",
                new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                "Actions",
                "{controller}/{action}",
                new { }
            );

            routes.MapRoute(
                "Actions2",
                "{controller}/{action}/{username}/{imageId}",
                new { });
        }
    }
}
