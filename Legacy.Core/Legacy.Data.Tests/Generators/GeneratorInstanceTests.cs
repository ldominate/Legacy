using Autofac;
using Legacy.Data.Tests.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Legacy.Data.Tests.Generators
{
	[TestClass]
	public class GeneratorInstanceTests : TestBase
	{
		[TestMethod]
		public void ShouldInstanceSimpleTypeAllGeneratorFromIoC()
		{
			Assert.IsNotNull(Container.Resolve<Fixture>());

			Assert.IsNotNull(Container.Resolve<StringGenerator>());
		}

		[TestMethod]
		public void ShouldInstanceEntityAllGeneratorFromIoC()
		{
			Assert.IsNotNull(Container.Resolve<OperationGenerator>());
		}
	}
}