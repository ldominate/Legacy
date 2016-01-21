using System.Data;
using System.Data.SqlClient;
using Legacy.Domain.Common;

namespace Legacy.Data
{
	public class AdoNetWorker : IWorker
	{
		private readonly SqlCommand _command;
		private readonly SqlTransaction _transaction;

		private bool _isOpen = true;

		public AdoNetWorker(SqlCommand command)
		{
			_command = command;
			_command.Transaction = _command.Connection.BeginTransaction();
			_transaction = _command.Transaction;
		}

		/// <summary>Выполнить запрос к БД без возврата результата</summary>
		/// <param name="query">Запрос</param>
		/// <param name="parameters">Параметры</param>
		/// <returns>Количество задействованных к инструкции строк</returns>
		public int ExecCrudQuery(string query, params SqlParameter[] parameters)
		{
			ExecQuerySet(query, parameters);

			return _command.ExecuteNonQuery();
		}

		/// <summary>Выполнить запрос к БД и вернуть значение первой строки первой колонки</summary>
		/// <typeparam name="TScalar">Тип возвращаемого значения</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="parameters">Параметры</param>
		/// <returns></returns>
		public TScalar ExecScalarQuery<TScalar>(string query, params SqlParameter[] parameters)
		{
			ExecQuerySet(query, parameters);

			return (TScalar) (_command.ExecuteScalar() ?? default(TScalar));
		}

		/// <summary>Выполняет запрос к БД и возвращает результат</summary>
		/// <param name="query">Запрос</param>
		/// <param name="parameters">Параметры</param>
		/// <returns></returns>
		public SqlDataReaderAdapter ExecSelectQuery(string query, params SqlParameter[] parameters)
		{
			ExecQuerySet(query, parameters);

			return new SqlDataReaderAdapter(_command.ExecuteReader());
		}

		/// <summary>Конфигурирует инструкцию SQL для выполнения запроса</summary>
		/// <param name="query">Запрос</param>
		/// <param name="parameters">Параметры</param>
		void ExecQuerySet(string query, params SqlParameter[] parameters)
		{
			_command.CommandType = CommandType.Text;
			_command.CommandText = query.Trim();
			_command.Parameters.Clear();

			if (parameters != null)
			{
				_command.Parameters.AddRange(parameters);
			}
		}

		public void Dispose()
		{
			if (_isOpen)
			{
				_command.Dispose();
				_isOpen = false;
			}
		}

		public void Commit()
		{
			if (_isOpen)
			{
				_transaction.Commit();
			}
		}

		public void Rollback()
		{
			if (_isOpen)
			{
				_transaction.Rollback();
			}
		}
	}
}