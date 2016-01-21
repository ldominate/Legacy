using System;
using System.Data;
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
				operation.Id = _worker.ExecScalarQuery<int>("INSERT INTO [Entity].[Operation] (Type, GroupId, Name) OUTPUT inserted.Id AS Id VALUES(@Type, @GroupId, @Name)",
					new SqlParameter("@Type", operation.Type),
					new SqlParameter("@GroupId", SqlDbType.Int){ Value = (object)operation.GroupId ?? DBNull.Value},
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
			try
			{
				using (var aReader = _worker.ExecSelectQuery("SELECT * FROM [Entity].[Operation] WHERE [Id] = @Id", new SqlParameter("@Id", id)))
				{
					Operation operation = null;

					if (aReader.Reader.Read())
					{
						operation = new Operation
						{
							Id = aReader.GetQueryValue("Id", -1),
							GroupId = aReader.GetQueryValue<int?>("GroupId"),
							Type = aReader.GetQueryValue("Type", OperationType.Item),
							Name = aReader.GetQueryValue<string>("Name"),
							IsDeleted = aReader.GetQueryValue("IsDeleted", false)
						};
					}
					return new ExecuteStatus<Operation>(operation);
				}
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<Operation>(null, ex.Message);
			}
		}
	}
}