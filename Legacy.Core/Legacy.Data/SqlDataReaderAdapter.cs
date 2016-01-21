using System;
using System.Data.SqlClient;

namespace Legacy.Data
{
	public class SqlDataReaderAdapter : IDisposable
	{
		private readonly SqlDataReader _reader;

		public SqlDataReaderAdapter(SqlDataReader reader)
		{
			_reader = reader;
		}

		public SqlDataReader Reader { get { return _reader; } }

		public TType GetQueryValue<TType>(string column, TType defaultValue = default(TType))
		{
			try
			{
				return (TType) (_reader[column] == DBNull.Value ? defaultValue : _reader[column]);
			}
			catch
			{
				return defaultValue;
			}
		}

		public void Dispose()
		{
			_reader.Dispose();
		}
	}
}