﻿using System;
using System.Data.SqlClient;

namespace Legacy.Data
{
	public class AgentWorker : IDisposable
	{
		private readonly SqlConnection _connection;

		public AgentWorker(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool isDisposable)
		{
			if (isDisposable)
			{
				_connection.Dispose();
			}
		}

		~AgentWorker()
		{
			Dispose(false);
		}
	}
}