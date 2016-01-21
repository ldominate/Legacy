using Legacy.Domain.Common;

namespace Legacy.Data
{
	/// <summary>Базовый класс провайдера</summary>
	public abstract class ProviderBase : IProviderBase
	{
		private readonly AdoNetWorker _worker;

		protected ProviderBase(AdoNetWorker worker)
		{
			_worker = worker;
		}
	}
}