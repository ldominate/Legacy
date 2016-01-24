using Legacy.Domain.Common.Requests;

namespace Legacy.Domain.Operations
{
	/// <summary>Запрос последовательности услуг по заданным параметрам фильтрации и сортировки</summary>
	public class OperationListRequest : ListLogicRequestBase
	{
		public OperationListRequest()
		{
			PropertySort = "Name";
		}

		public OperationType? Type { get; set; }

		public int? GroupId { get; set; }

		public string Name { get; set; }

	}
}