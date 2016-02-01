using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Legacy.Domain.Common;
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

		public Operation Create(SqlDataReaderAdapter aReader)
		{
			return new Operation
			{
				Id = aReader.GetQueryValue("Id", -1),
				GroupId = aReader.GetQueryValue<int?>("GroupId"),
				Type = aReader.GetQueryValue("Type", OperationType.Item),
				Level = aReader.GetQueryValue("Level", 0),
				Name = aReader.GetQueryValue<string>("Name"),
				Order = aReader.GetQueryValue("Order", 0),
				IsDeleted = aReader.GetQueryValue("IsDeleted", false)
			};
		}

		public ExecuteStatus<int> Add(Operation operation)
		{
			try
			{	//Добавление записи запросом
				operation.Id = _worker.ExecScalarQuery<int>(
					string.Format("INSERT INTO {0}.{1} ([Type], [GroupId], [Level], [Name], [Order]) OUTPUT inserted.Id AS Id VALUES(@Type, @GroupId, @Level, @Name, @Order)", ShemaName, TableName),
					CreateParameters(operation, new SqlParameter("@Level", operation.Level), new SqlParameter("@Order", operation.Order)).ToArray());

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
			return ChangeIsDeleted(id);
		}

		public ExecuteStatus LogicRecovery(int id)
		{
			return ChangeIsDeleted(id, false);
		}

		public ExecuteStatus ChangeIsDeleted(int id, bool isDeleted = true)
		{
			try
			{
				if (_worker.ExecCrudQuery(
					string.Format("UPDATE {0}.{1} SET [IsDeleted] = @IsDeleted WHERE [Id] = @Id", ShemaName, TableName),
					new SqlParameter("@Id", id), new SqlParameter("@IsDeleted", isDeleted)) > 0)
				{
					return new ExecuteStatus();
				}
				return new ExecuteStatus { Success = false };
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<int>(-1, ex.Message);
			}
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
				using (var aReader = _worker.ExecSelectQuery(string.Format("SELECT TOP 1 * FROM {0}.{1} WHERE [Id] = @Id", ShemaName, TableName), new SqlParameter("@Id", id)))
				{
					Operation operation = null;

					if (aReader.Reader.Read())
					{
						operation = Create(aReader);
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

		public ExecuteStatusList<IEnumerable<Operation>> GetList(OperationListRequest request)
		{
			try
			{
				using (var aReader = _worker.ExecProcReader("[Entity].[GetListOperation]",
					new SqlParameter("@startIndex", request.StartIndex),
					new SqlParameter("@count", request.Count),
					request.GroupId.HasValue ? new SqlParameter("@GroupId", request.GroupId.Value) : null,
					request.Type.HasValue ? new SqlParameter("@Type", request.Type.Value) : null,
					string.IsNullOrWhiteSpace(request.Name) ? null : new SqlParameter("@Name", string.Format("%{0}%", request.Name)),
					new SqlParameter("@Order", (request.Order == SortingOrdered.Desc)),
					new SqlParameter("@PropertySort", request.PropertySort)))
				{
					if (!aReader.Reader.Read())
					{
						return new ExecuteStatusList<IEnumerable<Operation>>(null, "No selected data");
					}
					var exStat = new ExecuteStatusList<IEnumerable<Operation>>(aReader.GetQueryValue("allCount", 0), null);

					if (aReader.Reader.NextResult())
					{
						var operations = new List<Operation>();

						while (aReader.Reader.Read())
						{
							operations.Add(Create(aReader));
						}
						exStat.Result = operations;
					}
					return exStat;
				}
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatusList<IEnumerable<Operation>>(null, ex.Message);
			}
		}

		public ExecuteStatus<int> MaxOrder(int groupId)
		{
			try
			{
				return new ExecuteStatus<int>(
					_worker.ExecScalarQuery<int>(string.Format("SELECT MAX([Order]) FROM {0}.{1} WHERE [GroupId] = @GroupId GROUP BY [GroupId]", ShemaName, TableName),
					new SqlParameter("@GroupId", groupId)));
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<int>(0, ex.Message);
			}
		}

		public ExecuteStatus SetOrder(Operation operation)
		{
			try
			{
				_worker.ExecCrudQuery(string.Format("UPDATE {0}.{1} SET [Order] = @Order WHERE [Id] = @Id", ShemaName, TableName),
					new SqlParameter("@Id", operation.Id),
					new SqlParameter("@Order", operation.Order));
				return new ExecuteStatus();
			}
			catch (Exception ex)
			{
				_worker.Rollback();

				return new ExecuteStatus<int>(0, ex.Message);
			}
		}
	}
}