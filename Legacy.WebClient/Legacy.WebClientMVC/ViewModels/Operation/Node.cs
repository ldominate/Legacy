using Legacy.Domain.Operations;

namespace Legacy.WebClientMVC.ViewModels.Operation
{
	public class Node
	{
		public Node()
		{
			
		}

		public Node(Domain.Operations.Operation operation)
		{
			id = operation.Id;
			name = operation.Name;
			type = (operation.Type == OperationType.Group) ? "default" : "";
		}

		public int id { get; set; }

		public string name { get; set; }

		public int level { get; set; }

		public string type { get; set; }
	}
}