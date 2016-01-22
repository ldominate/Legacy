namespace Legacy.Domain.Common.Requests
{
	/// <summary>Интерфейс запроса последовательностей элементов</summary>
	public interface IListRequest
	{
		/// <summary>Свойство сортировки</summary>
		string PropertySort { get; set; }

		/// <summary>Направление сортировки</summary>
		SortingOrdered Order { get; set; }
	}
}