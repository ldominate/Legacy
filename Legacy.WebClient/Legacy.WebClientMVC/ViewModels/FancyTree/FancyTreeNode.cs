using Legacy.Domain.Operations;

namespace Legacy.WebClientMVC.ViewModels.FancyTree
{
	public class FancyTreeNode
	{
		public FancyTreeNode()
		{
		}

		public FancyTreeNode(Domain.Operations.Operation operation)
		{
			key = operation.Id.ToString();

			title = operation.Name;

			folder = operation.Type == OperationType.Group || operation.Type == OperationType.Subgroup;

			lazy = folder;
		}

		public string title { get; set; }

		public string key { get; set; }

		public bool folder { get; set; }

		public FancyTreeNode[] children { get; set; }

		public bool lazy { get; set; }
	}
}