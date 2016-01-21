using System;

namespace Legacy.Domain.Common
{
	/// <summary>Рабочая единица</summary>
	public interface IWorker : IDisposable
	{
		/// <summary>Завфиксировать изменения и завершить транзакцию</summary>
		void Commit();

		/// <summary>Отменить изменения и завершить транзакцию</summary>
		void Rollback();
	}
}