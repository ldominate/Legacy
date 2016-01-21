using Legacy.Domain.Operations;
using Legacy.Domain.Results;

namespace Legacy.Data.Operations
{
	public class OperationProvider : ProviderBase, IOperationProvider
	{
		private readonly AdoNetWorker _worker;

		public OperationProvider(AdoNetWorker worker) : base(worker)
		{
			_worker = worker;
		}

		public ExecuteStatus<int> Add(Operation operation)
		{
			throw new System.NotImplementedException();
		}

		public ExecuteStatus<Operation> GetById(int id)
		{
			throw new System.NotImplementedException();
		}
	}
}