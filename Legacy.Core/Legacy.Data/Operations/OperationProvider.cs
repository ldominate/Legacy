using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Legacy.Domain.Operations;
using Legacy.Domain.Results;

namespace Legacy.Data.Operations
{
	public class OperationProvider : ProviderBase, IOperationProvider
	{
		private const string ShemaName = "[Entity]";
		private const string TableName = "[Operation]";

		private readonly AdoNetWorker _worker;

		public OperationProvider(AdoNetWorker worker) : base(worker)
		{
			_worker = worker;
		}

		public IEnumerable<SqlParameter> CreateParameters(Operation operation, params SqlParameter[] additionalParameters)
		{
			var @params = new List<SqlParameter>
			{
				new SqlParameter("@Type", operation.Type),
				new SqlParameter("@GroupId", SqlDbType.Int){ Value = (object)operation.GroupId ?? DBNull.Value},
				new SqlParameter("@Name", operation.Name)
			};
			if (additionalParameters != null && additionalParameters.Any())
			{
				@params.AddRange(additionalParameters);
			}
			return @params;
		}

		public ExecuteStatus<int> Add(Operation operation)
		{
			try
			{	//Добавление записи запросом
				operation.Id = _worker.ExecScalarQuery<int>(
					string.Format("INSERT INTO {0}.{1} ([Type], [GroupId], [Name]) OUTPUT inserted.Id AS Id VALUES(@Type, @GroupId, @Name)", ShemaName, TableName),
					CreateParameters(operation).ToArray());

				return new ExecuteStatus<int>(operation.Id);
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<int>(-1, ex.Message);
			}
		}

		public ExecuteStatus Update(Operation operation)
		{
			try
			{	//Изменение данных записи через хранимую процедуру
				_worker.ExecProcNonReader("[Entity].[UpdateOperation]",
					CreateParameters(operation, new SqlParameter("@Id", operation.Id)).ToArray());

				return new ExecuteStatus();
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<int>(-1, ex.Message);
			}
		}

		public ExecuteStatus LogicDelete(int id)
		{
			throw new NotImplementedException();
		}

		public ExecuteStatus LogicRecovery(int id)
		{
			throw new NotImplementedException();
		}

		public ExecuteStatus ForceDelete(int id)
		{
			try
			{
				if (_worker.ExecCrudQuery(
					string.Format("DELETE FROM {0}.{1} WHERE [Id] = @Id", ShemaName, TableName), new SqlParameter("@Id", id)) > 0)
				{
					return new ExecuteStatus();
				}
				return new ExecuteStatus {Success = false};
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
				using (var aReader = _worker.ExecSelectQuery("SELECT TOP 1 * FROM [Entity].[Operation] WHERE [Id] = @Id", new SqlParameter("@Id", id)))
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