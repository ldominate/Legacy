using Autofac;
using Legacy.Domain.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Legacy.Data.Tests.IoConfiguration
{
	[TestClass]
	public class ProviderInstanceTests : TestBase
	{

		[TestMethod]
		public void ShouldInstanceAllProvideerFromIoC()
		{
			Assert.IsNotNull(Container.Resolve<IOperationProvider>());
		}

	}
}