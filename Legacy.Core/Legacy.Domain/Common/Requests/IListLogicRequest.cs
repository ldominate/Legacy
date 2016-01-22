namespace Legacy.Domain.Common.Requests
{
	/// <summary>Интерфейс запроса последовательности с фильтром логически удалённых элементов</summary>
	public interface IListLogicRequest
	{
		/// <summary>Получить отмеченные на удаление элементы</summary>
		bool? IsDelete { get; set; }
	}
}