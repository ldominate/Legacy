using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		public int? GroupId { get; set; }

		/// <summary>Наименование</summary>
		[Required, StringLength(255)]
		public string Name { get; set; }

		/// <summary>Дочерние элементы</summary>
		public ICollection<Operation> Operations { get; set; }
	}
}