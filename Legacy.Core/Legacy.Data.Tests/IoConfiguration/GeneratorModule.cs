using Autofac;
using Ploeh.AutoFixture;
using StringGenerator = Legacy.Data.Tests.Generators.StringGenerator;

namespace Legacy.Data.Tests.IoConfiguration
{
	public class GeneratorModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<Fixture>().SingleInstance();

			builder.RegisterTypes(
				typeof(StringGenerator))
				.SingleInstance();
		}
	}
}