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

		/// <summary>Обозначает уровень вложенности услуги</summary>
		public int Level { get; set; }

		/// <summary>Наименование</summary>
		[Required, StringLength(255)]
		public string Name { get; set; }

		/// <summary>Обозначает порядок сортировки</summary>
		public int Order { get; set; }

		/// <summary>Дочерние элементы</summary>
		public ICollection<Operation> Operations { get; set; }

		public Operation SetParentByChilds()
		{
			foreach (var child in Operations)
			{
				child.GroupId = Id;
			}
			return this;
		}
	}
}