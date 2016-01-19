using System;

namespace Legacy.Domain.Common
{
	/// <summary>Базовый класс</summary>
	public abstract class BaseDomainObject : IEquatable<BaseDomainObject>
	{

		/// <summary>Идентификатор объекта</summary>
		public int Id { get; set; }

		public bool Equals(BaseDomainObject other)
		{
			return other != null && other.Id == Id && other.GetType() == GetType();
		}
	}
}