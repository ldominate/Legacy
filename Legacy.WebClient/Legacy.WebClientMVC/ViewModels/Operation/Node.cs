using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
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
			parent = operation.GroupId ?? 0;
			name = operation.Name;
			level = operation.Level;
			type = (operation.Type == OperationType.Group) ? "default" : "";
		}

		public int id { get; set; }

		[ScriptIgnore]
		public int parent { get; set; }

		[Required, StringLength(255)]
		public string name { get; set; }

		public int level { get; set; }

		public string type { get; set; }

		[ScriptIgnore]
		public string position { get; set; }

		[ScriptIgnore]
		public int related { get; set; }

		[ScriptIgnore]
		public Domain.Operations.Operation Operation
		{
			get
			{
				return new Domain.Operations.Operation
				{
					Id = id,
					GroupId = parent == 0 ? null : (int?)parent,
					Name = name,
					Level = level,
					IsDeleted = false
				};
			}
		}
	}
}