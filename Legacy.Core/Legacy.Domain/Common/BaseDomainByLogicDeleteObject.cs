namespace Legacy.Domain.Common
{
	/// <summary>Базовый класс с флагом логического удаления</summary>
	public abstract class BaseDomainByLogicDeleteObject : BaseDomainObject
	{
		/// <summary>Флаг логического удаления</summary>
		public bool IsDeleted { get; set; }
	}
}