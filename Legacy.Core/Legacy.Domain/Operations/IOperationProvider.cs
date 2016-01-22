﻿using Legacy.Domain.Common;
using Legacy.Domain.Results;

namespace Legacy.Domain.Operations
{
	/// <summary>Интерфейс провайдера доступа к данным справочника номенклатуры услуг</summary>
	public interface IOperationProvider : IProviderBase
	{
		/// <summary>Добавляет услугу в БД</summary>
		/// <param name="operation">Номенклатура услуг</param>
		/// <returns>Результат исполнения с присвоеным идентификатором</returns>
		ExecuteStatus<int> Add(Operation operation);

		/// <summary>Обновить данные услуги в БД</summary>
		/// <param name="operation">Номенклатура услуг</param>
		/// <returns>Результат исполнения</returns>
		ExecuteStatus Update(Operation operation);

		/// <summary>Пометить услугу на удаление</summary>
		/// <param name="id">Идентификатор услуги</param>
		/// <returns>Результат исполнения</returns>
		ExecuteStatus LogicDelete(int id);

		/// <summary>Снять отметку на удаление</summary>
		/// <param name="id">Идентификатор услуги</param>
		/// <returns>Результат исполнения</returns>
		ExecuteStatus LogicRecovery(int id);

		/// <summary>Удалить услугу из БД</summary>
		/// <param name="id">Идентификатор услуги</param>
		/// <returns>Результат исполнения</returns>
		ExecuteStatus ForceDelete(int id);

		/// <summary>Получить услугу из БД по заданному идентификатору</summary>
		/// <param name="id">Идентификатор услуги</param>
		/// <returns>Номенклатура услуг</returns>
		ExecuteStatus<Operation> GetById(int id);
	}
}