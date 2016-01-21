using System;
using System.Data.SqlClient;
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
			try
			{
				operation.Id = _worker.ExecScalarQuery<int>("INSERT INTO [Entity].[Operation] (Type, GroupId, Name) VALUES(@Type, @GroupId, @Name)",
					new SqlParameter("@Type", operation.Type),
					new SqlParameter("@GroupId", operation.GroupId),
					new SqlParameter("@Name", operation.Name));

				return new ExecuteStatus<int>(operation.Id);
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<int>(-1, ex.Message);
			}
		}

		public ExecuteStatus<Operation> GetById(int id)
		{
			throw new System.NotImplementedException();
		}
	}
}