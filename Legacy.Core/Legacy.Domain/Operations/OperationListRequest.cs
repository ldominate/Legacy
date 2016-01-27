using Legacy.Domain.Common.Requests;

namespace Legacy.Domain.Operations
{
	/// <summary>Запрос последовательности услуг по заданным параметрам фильтрации и сортировки</summary>
	public class OperationListRequest : ListLogicRequestBase
	{
		public OperationListRequest()
		{
			PropertySort = "Order";

			Type = OperationType.Group;
		}

		public OperationType? Type { get; set; }

		public int? GroupId { get; set; }

		public string Name { get; set; }

	}
}