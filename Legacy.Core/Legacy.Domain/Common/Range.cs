namespace Legacy.Domain.Common
{
	/// <summary>Диапазон значений</summary>
	/// <typeparam name="T">Тип ограничения диапазона</typeparam>
	public class Range<T>
	{
		public Range()
		{
			From = default(T);
			To = default(T);
		}

		public Range(T @from, T to)
			: this()
		{
			From = @from;
			To = to;
		}

		public T To { get; set; }

		public T From { get; set; }
	}
}