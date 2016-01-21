using System.Collections.Generic;
using Legacy.Domain.Common;

namespace Legacy.Domain.Operations
{
	/// <summary>Номенклатура услуг</summary>
	public class Operation : BaseDomainByLogicDeleteObject
	{
		public Operation()
		{
			Operations = new HashSet<Operation>();
		}

		/// <summary>Тип номенклатуры услуг</summary>
		public OperationType Type { get; set; }

		/// <summary>Идентификатор группы</summary>
		public int GroupID { get; set; }

		/// <summary>Наименование</summary>
		public string Name { get; set; }

		public ICollection<Operation> Operations { get; set; }
	}
}