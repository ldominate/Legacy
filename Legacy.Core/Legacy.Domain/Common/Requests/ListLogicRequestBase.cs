namespace Legacy.Domain.Common.Requests
{

	public abstract class ListLogicRequestBase : IListRequest, IListPagedRequest, IListLogicRequest
	{
		public ListLogicRequestBase()
		{
			Count = 100;

			IsDelete = false;
		}

		public string PropertySort { get; set; }

		public SortingOrdered Order { get; set; }

		public int StartIndex { get; set; }

		public int Count { get; set; }

		public bool? IsDelete { get; set; }
	}
}