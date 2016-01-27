using System.Configuration;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Legacy.Data;
using Legacy.Domain.Common;
using Legacy.WebClientMVC.IoCModules;
using Microsoft.Owin;
using Owin;

namespace Legacy.WebClientMVC
{
	[assembly: OwinStartup(typeof(Startup))]
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var builder = new ContainerBuilder();

			builder.Register(c => new AgentWorker(ConfigurationManager.ConnectionStrings["dbConnection"].ToString()))
				.SingleInstance();

			builder.Register(c => c.Resolve<AgentWorker>().CreateWorker())
				.As<AdoNetWorker, IWorker>()
				.OnRelease(worker =>
				{
					worker.Commit();
					worker.Dispose();
				})
				.InstancePerRequest();

			builder.RegisterModule<ProviderModule>();

			// Register web abstractions like HttpContextBase.
			builder.RegisterModule<AutofacWebTypesModule>();

			// Register your MVC controllers.
			builder.RegisterControllers(typeof(Global).Assembly);

			var container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			app.UseAutofacMiddleware(container);
		}
	}
}