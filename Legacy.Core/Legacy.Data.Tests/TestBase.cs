using Autofac;
using Legacy.Data.Tests.IoConfiguration;
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

			builder.RegisterModule<ProviderModule>();

			Container = builder.Build();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Container.Dispose();
		}
	}
}