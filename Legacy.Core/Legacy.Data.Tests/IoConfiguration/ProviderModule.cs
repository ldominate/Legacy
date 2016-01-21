using Autofac;
using Legacy.Data.Operations;
using Legacy.Domain.Operations;

namespace Legacy.Data.Tests.IoConfiguration
{
	public class ProviderModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<OperationProvider>().As<IOperationProvider>();
		}
	}
}