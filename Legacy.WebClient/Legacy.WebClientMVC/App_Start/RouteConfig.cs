using System.Web.Mvc;
using System.Web.Routing;

namespace Legacy.WebClientMVC
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("OperationMain", "operation", new {controller = "Operation", action = "Index"});
			routes.MapRoute("OperationNodes", "operation/nodes", new { controller = "Operation", action = "Nodes" });

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}