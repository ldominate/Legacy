using System.Collections.Generic;
using Legacy.Domain.Common;
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

		/// <summary>Получить последовательность элементов из БД по заданным параметрам запроса</summary>
		/// <param name="request">Запрос</param>
		/// <returns>Результат запроса с последовательностью услуг</returns>
		ExecuteStatusList<IEnumerable<Operation>> GetList(OperationListRequest request);

		/// <summary>Возвращает максимальный индекс сортировки для заданной группы</summary>
		/// <param name="groupId">Идентификатор группы</param>
		/// <returns>Максимальный индекс сортировки</returns>
		ExecuteStatus<int> MaxOrder(int groupId);

		/// <summary>Задаёт порядок сортировки по указанному идентификатору услуги</summary>
		/// <param name="operation">Номенклатура услуг</param>
		/// <returns>Результат запроса</returns>
		ExecuteStatus SetOrder(Operation operation);
	}
}