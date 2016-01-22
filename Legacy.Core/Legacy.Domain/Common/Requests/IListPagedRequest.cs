namespace Legacy.Domain.Common.Requests
{
	/// <summary>Интерфейс запроса последовательности элементов с параметрами навигации</summary>
	public interface IListPagedRequest
	{
		/// <summary>Стартовый индекс загрузки</summary>
		int StartIndex { get; set; }

		/// <summary>Количество элементов загрузки</summary>
		int Count { get; set; }
	}
}