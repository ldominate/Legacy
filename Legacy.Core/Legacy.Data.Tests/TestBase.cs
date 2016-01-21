using System.Configuration;
using Autofac;
using Legacy.Data.Tests.IoConfiguration;
using Legacy.Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Legacy.Data.Tests
{
	public abstract class TestBase
	{
		protected IContainer Container { get; private set; }

		[TestInitialize]
		public void Initialization()
		{
			var builder = new ContainerBuilder();

			builder.Register(c => new AgentWorker(ConfigurationManager.ConnectionStrings["dbConnection"].ToString()))
				.SingleInstance();

			builder.Register(c => c.Resolve<AgentWorker>().CreateWorker())
				.As<AdoNetWorker, IWorker>()
				.OnRelease(worker =>
				{
					worker.Rollback();
					worker.Dispose();
				})
				.SingleInstance();

			builder.RegisterModule<ProviderModule>();
			builder.RegisterModule<GeneratorModule>();

			Container = builder.Build();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Container.Dispose();
		}
	}
}